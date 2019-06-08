#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <atomic>

using namespace std;
using namespace std::chrono;

atomic<int> sum1;	// 변수를 사용할 때 아토믹하게 연산한다.
atomic<int> sum2;

void Thread_Func1()
{
	for( auto i = 0; i < 500000; ++i )
	{
		sum1.fetch_add(2);
	}
}

void Thread_Func2()
{
	for( auto i = 0; i < 500000; ++i )
	{
		sum2.fetch_add(2);
	}
}

void Thread_Func3()
{
	for( auto i = 0; i < 50; ++i )
	{
		sum1.fetch_add(sum2);		// <atomic> + <atomic>
	}
}

int main()
{
	vector<thread *> worker_threads;

	for( auto i = 1; i <= 32; i += i )
	{
		sum1 = 0;
		sum2 = 0;
		worker_threads.clear();

		auto start = high_resolution_clock::now();

		for( auto j = 0; j < i; ++j )
		{
			worker_threads.emplace_back(new thread{ Thread_Func1 });
			worker_threads.emplace_back(new thread{ Thread_Func2 });
			worker_threads.emplace_back(new thread{ Thread_Func3 });
		}

		for( auto t : worker_threads )
			t->join();

		auto du = high_resolution_clock::now() - start;

		cout << "threads : " << i << " / sum : " << sum << " / ";

		cout << "time : " << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " s" << endl;
	}
}

// 아토믹으로 선언된 두 변수끼리 연산을 하면 결과는 올바르게 나오지 않는다.
// <atomic> + <atomic> != <atomic> 으로 결론이 나온다.
// 그렇다면 어떻게 해야 성능을 올리면서 올바른 결과값을 나오게 할 수 있을까?