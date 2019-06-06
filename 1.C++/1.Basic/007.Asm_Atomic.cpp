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
		_asm lock add sum, 2;	// x86 ����� prefix�� ����Ͽ�, ������(atomic)���� �۾��� �Ѵ�.
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

		cout << "������ ���� : " << i << " / sum : " << sum << " / ";

		cout << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " ��s\n";
	}
}

// sum += 2;�� ������¿��� ����ε� ������� �� �� ������.
// ��ȣ���縦 ����ϸ� ������� ������ �� ������ �ӵ��� ��������.
// ��� ��ȣ����� ���� �ӵ� ���ϸ� �ذ��ϸ� �ȴ�.
// �ϳ��� ������� ������(atomic)���� �����ϸ� �ȴ�.
// ��ȣ���縦 ������� �ʰ� ����� �ùٸ��� �̲��� �´�.
// ������ ��ȣ���縦 ������� ������ �ӵ��� ����������, ������ �����尡 �������� �� ������ �϶��Ѵ�.
// ������ ������ �� ���� ������ ��Ű�� ������ �����尡 �������� �̱۽������ �̱۽������ �ٸ��� ���� �۾��� �Ǿ��� �����̴�.
// �����尡 ������ ���Ʊ�ȯ �۾����� �߰��� ��� ������ �����尡 �������� ������ �϶��ϰ� �ȴ�.