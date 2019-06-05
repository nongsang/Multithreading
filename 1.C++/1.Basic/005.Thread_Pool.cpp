#include <iostream>
#include <thread>
#include <chrono>
#include <vector>

using namespace std;
using namespace std::chrono;

int sum;

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )	// 스레드의 갯수만큼 전체 할 일을 나누어서 실행한다.
	{													// 스레드가 많아지면 경쟁상태는 더욱 심해진다.
		sum += 2;
	}
}

int main()
{
	vector<thread *> worker_threads;	// 스레드풀을 만들어 작업중인 스레드들을 저장해놓는다.
										// 작업중인 스레드가 몇개 있는지도 확인이 쉽다.

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

		cout << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " μs\n";	// 시간 출력
	}
}

// 스레드가 많아질 수록 결과값은 더 이상해진다.
// 경쟁상태가 더 심해지기 때문이다.
// 만약 작업을 하나의 스레드만 접근하게 해서 연산을 하게 하고 나머지 스레드는 대기를 하게하면 결과값이 옳게 나오겠지?