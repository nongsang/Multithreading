#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <atomic>

using namespace std;
using namespace std::chrono;

atomic<int> sum1;	// ������ ����� �� ������ϰ� �����Ѵ�.
atomic<int> sum2;

void Thread_Func1()
{
	for( auto i = 0; i < 10; ++i )
	{
		sum1.fetch_add(1);
	}
}

void Thread_Func2()
{
	for( auto i = 0; i < 10; ++i )
	{
		sum2.fetch_add(1);
	}
}

void Thread_Func3()
{
	for( auto i = 0; i < 10; ++i )
	{
		sum1.fetch_add(sum2);	// <atomic> + <atomic>
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

// ��������� ����� �� �������� ������ �ϸ� ����� �ùٸ��� ������ �ʴ´�.
// <atomic> + <atomic> != <atomic> ���� ����� ���´�.
// �׷��ٸ� ��� �ؾ� ������ �ø��鼭 �ùٸ� ������� ������ �� �� ������?