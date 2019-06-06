#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <mutex>

using namespace std;
using namespace std::chrono;

mutex mylock;	// ��ȣ����
int sum;

void Thread_Func(int num_threads)
{
	for( auto i = 0; i < 50000000 / num_threads; ++i )
	{
		mylock.lock();		// �ѹ��� �Ѱ��� �����常 ������ �� �ְ� ���
							// �ϳ��� �����常 ����� ��� ���� �۾��� �� �� �ִ�.
							// ������ ��������� ť(Queue)�� ������ ���� �����ϸ� ���

		sum += 2;			// �б�, ����, ���� 3�� �۾��� �� �ٿ� �� �ִ�.
							// ���� �۾��̱� ������ ���� ���� �ٸ� �����尡 ���� �� �ְ�, �б� ���� �ٸ� �����尡 ���⸦ �ϴ� ��찡 �ִ�.
							// ������ �ѹ��� �� �����常 �����ؼ� �۾��� �ϰ� �ϸ� �ùٸ� ����� ���� ���̴�.

		mylock.unlock();	// �۾��� ���� ������� ����� Ǭ��.
							// ť�� �ִ� �����尡 ���� ����� ��� �۾��� �Ѵ�.
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

		cout << "������ ���� : " << i << " / sum : " << sum << " / ";

		cout << static_cast<double>(duration_cast<milliseconds>(du).count()) / 1000 << " msecs\n";
	}
}

// �����带 ���� ���� �۾��� ���Ѽ� ������ �ö󰡱� ����ߴ�.
// ������ �����尡 ���� ���� �ӵ��� �� ��������.
// ��ȣ���簡 ������ �ȴ�.
// ���� ���� ������ 1���� ������ �ϰ� �Ǵµ�, �׷��� �̱۽������ ������ �ϴ� �Ͱ� ���̰� ��������.
// �����尡 �������� ��������� �����ư��鼭 ������ �ϱ� ������ �ٸ� �����忡�� ���� ������ �Ѱ��ִ� ��ȯ�۾��� �߰��� �ȴ�.
// �� ������ ��ȯ �۾��� ���Ʊ�ȯ(Context Switching)�̶�� �Ѵ�.
// ��ȣ����� �̱۽������ �ٸ��� ���� �۾��� �Ǿ���, �����尡 ������ ���� ���Ʊ�ȯ�� �������� ������ ������ ���� ������ �� �ۿ� ����.