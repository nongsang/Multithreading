#include <iostream>
#include <thread>

using namespace std;

void Calc()
{
	cout << "Calc()" << endl;
}

void Add(int a, int b)
{
	cout << "Add(int, int) : " << a + b << endl;
}

void Sub(int a, int b)
{
	cout << "Sub(int, int) : " << a - b << endl;
}

class Adder	// ����
{
private:
	int a;
	int b;
public:
	Adder(int _a, int _b) : a(_a), b(_b) {}	// ������
	void operator()() const					// ����ȯ ������
	{
		cout << "Adder.operator() : " << a + b << endl;
	}
};

int main()
{
	thread t1 = thread{ Calc };		// �Ű������� ���� �Լ��� �����带 ������ �� �Űܺ����� �Լ� �̸��� �����ϸ� �ȴ�.

	void (*func)() = Calc;			// �Լ������� ����
	thread t2 = thread(func);		// thread �����ڷ� �Լ������͸� �������ش�.
									// �⺻������ ������ �������� �Ű������� �Լ������ͷ� ���ǰ� �Ǿ� ������ �� �� �ִ�.

	thread * t3 = new thread(func);	// �������� �����带 �����Ͽ� ����� �� �ִ�.

	thread t4 = thread(Add, 1, 2);	// C#�� �޸� �Ű������� 2�� �̻� �����Ͽ� �����带 ������ �� �ִ�.

	thread t5 = thread(Adder(3, 4));	// ���ͷ� ������ ���� ����

	thread t6 = thread([]() { cout << 5 - 6 << endl; });	// ���ٷ� ������ ���� ����

	thread t7 = thread([]() { Sub(7, 8); });	// ���� ���ο��� �Լ� ȣ��� ������ ���� ����

	t1.join();
	t2.join();
	t3->join();	// t3�� ���������̹Ƿ� ��������� ���ٿ����ڴ� ->
	t4.join();
	t5.join();
	t6.join();
	t7.join();

	delete(t3);	// t3�� �������� ���������� �޸� ������ ����� �Ѵ�.
}

// ���� ����� ���׹����̴�.
// ���ķ� ����Ǵ� ��������� ������ ������� ������� ���� �����̴�.
// ��������� ����� ������ ���� ������� ���� ���� ������� ���̴�.
// C#�� �����带 ������ ������ ������� ���Ѽ� ����� ������ C++�� �ƴϴ�.