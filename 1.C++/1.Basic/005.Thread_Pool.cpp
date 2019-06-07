#include <iostream>
#include <thread>
#include <chrono>
#include <vector>

using namespace std;
using namespace std::chrono;

int sum;

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )	// �������� ������ŭ ��ü �� ���� ����� �����Ѵ�.
	{													// �����尡 �������� ������´� ���� ��������.
		sum += 2;
	}
}

int main()
{
	vector<thread *> worker_threads;	// ������Ǯ�� ����� �۾����� ��������� �����س��´�.
										// �۾����� �����尡 � �ִ����� Ȯ���� ����.

	for( auto i = 1; i <= 32; i += i )	// �����尡 1�� ~ 32���� �� ������ ����
	{
		sum = 0;
		worker_threads.clear();

		auto start = high_resolution_clock::now();	// �ð� ���� ����

		for( auto j = 0; j < i; ++j )									// ������ ������ŭ
			worker_threads.emplace_back(new thread{ Thread_Func, i });	// �����带 �����Ͽ� ������Ǯ�� ����

		for( auto t : worker_threads )	// ������Ǯ�� �ִ� ��� ������
			t->join();					// ���

		auto du = high_resolution_clock::now() - start;	// �ð� ���� ��

		cout << "threads : " << i << " / sum : " << sum << " / ";

		cout << "time : " << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " s" << endl;	// �ð� ���
	}
}

// �����尡 ������ ���� ������� �� �̻�������.
// ������°� �� �������� �����̴�.
// ���� �۾��� �ϳ��� �����常 �����ϰ� �ؼ� ������ �ϰ� �ϰ� ������ ������� ��⸦ �ϰ��ϸ� ������� �ǰ� ��������?