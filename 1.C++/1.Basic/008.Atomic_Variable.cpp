#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <atomic>

using namespace std;
using namespace std::chrono;

atomic<int> sum;	// C++11���� ����� �ڷ����� ������ �� �ִ�.

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )
	{
		//sum += 2;			// �Ϲ����� ���굵 �����ϴ�.
		sum.fetch_add(2);	// ���� ��ü������ ����� ������ �����Ѵ�.
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

		for( auto j = 0; j < i; ++j )
			worker_threads.emplace_back(new thread{ Thread_Func, i });

		for( auto t : worker_threads )
			t->join();

		auto du = high_resolution_clock::now() - start;

		cout << "threads : " << i << " / sum : " << sum << " / ";

		cout << "time : " << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " ��s\n";
	}
}

// ������̶�
// 1. ��� Atomic ������ �ٸ� Atomic ����� ���ÿ� ������� �ʴ´�.(��ȣ����)
// 2. Atomic ������ ������ ��ɾ��� ������ ��� ���� �� ���� ���۵��� �ʴ´�.
// 3. ��� Atomic ������ ���� ������ ��� �����忡�� �����ϰ� �����ȴ�.

// ������� ��ĩ �������� ������ �� �ִ�.
// ������� ����ص� ������ ������ �߻��Ѵ�.