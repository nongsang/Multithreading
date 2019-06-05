#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <mutex>

using namespace std;
using namespace std::chrono;

mutex mylock;	// 상호배재
int sum;

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )	// 스레드의 갯수만큼 전체 할 일을 나누어서 실행한다.
	{
		mylock.lock();		// 한번에 한개의 스레드만 접근할 수 있게 잠금
							// 하나의 스레드만 잠금을 얻고 다음 작업을 할 수 있다.
							// 나머지 스레드들은 큐(Queue)에 저장해 놓고 스핀하며 대기

		sum += 2;			// 읽기, 연산, 쓰기 3개 작업이 한 줄에 다 있다.
							// 여러 작업이기 때문에 연산 도중 다른 스레드가 읽을 수 있고, 읽기 도중 다른 스레드가 쓰기를 하는 경우가 있다.
							// 때문에 한번에 한 스레드만 접근해서 작업을 하게 하면 올바른 결과가 나올 것이다.

		mylock.unlock();	// 작업을 끝낸 스레드는 잠금을 푼다.
							// 큐에 있는 스레드가 다음 잠금을 얻고 작업을 한다.
	}
}

int main()
{
	vector<thread *> worker_threads;	// 스레드풀

	for( auto i = 1; i <= 32; i += i )	// 스레드가 1개 ~ 32개일 때 성능을 측정
	{
		sum = 0;
		worker_threads.clear();

		auto start = high_resolution_clock::now();	// 시간 측정 시작

		for( auto j = 1; j <= i; ++j )								// 스레드 갯수만큼
			worker_threads.push_back(new thread{ Thread_Func, i });	// 스레드를 생성하여 스레드풀에 저장

		for( auto t : worker_threads )	// 스레드풀에 있는 모든 스레드
			t->join();					// 대기

		auto du = high_resolution_clock::now() - start;	// 시간 측정 끝

		cout << "스레드 갯수 : " << i << " / sum : " << sum << " / ";	// 결과 출력

		cout << static_cast<double>(duration_cast<milliseconds>(du).count()) / 1000 << " msecs\n";	// 시간 출력
	}
}

// 스레드를 많이 만들어서 작업을 시켜서 성능이 올라가길 기대했다.
// 하지만 스레드가 많을 수록 속도는 더 떨어진다.
// 상호배재가 문제가 된다.
// 락을 얻은 스레드 1개만 연산을 하게 되는데, 그러면 싱글스레드로 연산을 하는 것과 차이가 없어진다.
// 스레드가 많아지면 스레드들이 번갈아가면서 연산을 하기 때문에 다른 스레드에게 연산 권한을 넘겨주는 교환작업도 추가가 된다.
// 이 스레드 교환 작업을 문맥교환(Context Switching)이라고 한다.
// 상호배재로 싱글스레드와 다를바 없는 작업이 되었고, 스레드가 많아질 수록 문맥교환은 많아지기 때문에 성능은 더욱 떨어질 수 밖에 없다.