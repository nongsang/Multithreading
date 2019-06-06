#include <iostream>
#include <thread>
#include <chrono>
#include <vector>

using namespace std;
using namespace std::chrono;

int sum;

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )
	{
		_asm lock add sum, 2;	// x86 어셈블리 prefix를 사용하여, 원자적(atomic)으로 작업을 한다.
	}
}

int main()
{
	vector<thread *> worker_threads;

	for( auto i = 1; i <= 32; i += i )
	{
		sum = 0;
		worker_threads.clear();

		auto start = high_resolution_clock::now();

		for( auto j = 1; j <= i; ++j )
			worker_threads.emplace_back(new thread{ Thread_Func, i });

		for( auto t : worker_threads )
			t->join();

		auto du = high_resolution_clock::now() - start;

		cout << "스레드 갯수 : " << i << " / sum : " << sum << " / ";

		cout << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " μs\n";
	}
}

// sum += 2;는 경쟁상태에서 제대로된 결과값을 낼 수 없었다.
// 상호배재를 사용하면 결과값을 보장할 수 있지만 속도가 느려진다.
// 적어도 상호배재로 인한 속도 저하만 해결하면 된다.
// 하나의 연산식을 원자적(atomic)으로 선언하면 된다.
// 상호배재를 사용하지 않고도 결과를 올바르게 이끌어 냈다.
// 문제는 상호배재를 사용했을 때보다 속도는 나아졌으나, 여전히 스레드가 많아졌을 때 성능이 하락한다.
// 변수에 접근할 때 마다 순서를 지키기 때문에 스레드가 많아져도 싱글스레드와 싱글스레드와 다를바 없는 작업이 되었기 때문이다.
// 스레드가 많으면 문맥교환 작업까지 추가가 됬기 때문에 스레드가 많아지면 성능이 하락하게 된다.