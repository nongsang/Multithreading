#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <atomic>
//#include <mutex>

using namespace std;
using namespace std::chrono;

atomic<int> sum;	// ���� �Ѱ��� ���� ����(����, �������� ������ ����)�� ������ϰ� �ϰ� ������ ����ͺ��� ���
//mutex mylock;		// ���� �۾�(�Լ��� ���� ������ ������ ������)�� ������ϰ� �ϰ� ������ ��ȣ���� ���

void Optimal_Thread_Func(int num_threads)
{
	volatile int local_num = 0;	// volatile�� ����ȭ�� ���� �ʾƼ� �޸𸮿��� ���� �о���� ó���� ���� �����Ѵ�.
								// ����ȭ�� �ϸ� �ٷ� ������� ���� �� �ְ� �� ��������, �������� ������ ���� �ӵ��� ���������� Ȯ���ϱ� ���� ���

	for( auto i = 0; i < 500000000 / num_threads; ++i )
	{
		local_num += 2;	// volatile�� ��������Ƿ� 2, 4, 6 ... ������ �״�� �����Ѵ�.
						// volatile�� ������� ������ �ڵ� ����ȭ�� �Ͽ� 1���� local_num�� �ٷ� ������ �Ǽ� �ӵ��� ���������� Ȯ���ϱ� �����.
	}
	
	//mylock.lock();	// sum�� �׳� int���̸� lock�� ����ϸ� �ȴ�.
	sum += local_num;	// sum�� atomic<int>���̸� �׳� ���ص� �ȴ�.
	//mylock.unlock();	// ���� �߿��� ���� ���� �۾��� ó���ϰ� ����� �� �� ������ ��Ű�� ���踦 �ϸ� �ȴٴ� ���̴�.
						// ���� �۾����� lock�� �Ŵ� �ͺ��� ���ǿ��� lock�� �ɸ� �ӵ��� �ø��� ������� �ǰ� ���´�.
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
			worker_threads.emplace_back(new thread{ Optimal_Thread_Func, i });

		for( auto t : worker_threads )
			t->join();

		auto du = high_resolution_clock::now() - start;

		cout << "threads : " << i << " / sum : " << sum << " / ";

		cout << "time : " << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " s\n";
	}
}