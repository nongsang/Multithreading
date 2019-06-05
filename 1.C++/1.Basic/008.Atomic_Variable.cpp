#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <atomic>

using namespace std;
using namespace std::chrono;

atomic<int> sum;	// C++11부터 아토믹 자료형을 생성할 수 있다.

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )
	{
		//sum += 2;			// 일반적인 연산도 가능하다.
		sum.fetch_add(2);	// 물론 자체적으로 아토믹 연산을 지원한다.
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
			worker_threads.push_back(new thread{ Thread_Func, i });

		for( auto t : worker_threads )
			t->join();

		auto du = high_resolution_clock::now() - start;

		cout << "스레드 갯수 : " << i << " / sum : " << sum << " / ";

		cout << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " μs\n";
	}
}

// 상호배재를 사용하지 않는 아토믹은 자칫 만능으로 생각할 수 있다.
// 아토믹을 사용해도 문제는 여전히 발생한다.