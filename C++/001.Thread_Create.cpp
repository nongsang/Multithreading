#include <iostream>
#include <thread>
#include <chrono>

using namespace std;
using namespace chrono;

void Wait()
{
	cout << "���� ������ ����" << endl;

	this_thread::sleep_for(seconds(1));	// 1�ʰ� ���

	cout << "���� ������ ����" << endl;
}

int main()								// main() �Լ��� �����ϱ� ���ؼ� ������ 1���� ������ ����ϰ� �ȴ�.
{										// main() �Լ��� �����ϴ� �����带 ���� �������� �Ѵ�.
	cout << "���� ������ ����" << endl;

	thread t = thread{ Wait };			// ������ �ȿ��� ���ο� �����带 ������ �� ������, ������ �޼����� �̸��� �����ؾ� �Ѵ�.
										// ������ ������� ������ �Լ����� �����Ѵ�.
										// C#�� ���� �����带 foreground, background ������ �� ������ C�� C++�� background�θ� ���� ����

	t.join();							// t�� ���� ������ ���� ������� ����Ѵ�.
										// join�� ���� ������ ���� �����尡 ���� �����庸�� ���� �����ϰ� �ǹǷ� �����߻�

	cout << "���� ������ ����" << endl;
}