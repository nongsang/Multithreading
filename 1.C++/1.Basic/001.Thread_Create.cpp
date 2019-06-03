#include <iostream>
#include <thread>
#include <chrono>

using namespace std;
using namespace chrono;

void Wait()
{
	cout << "서브 스레드 시작" << endl;

	this_thread::sleep_for(seconds(1));	// 1초간 대기

	cout << "서브 스레드 종료" << endl;
}

int main()								// main() 함수를 실행하기 위해서 스레드 1개는 무조건 사용하게 된다.
{										// main() 함수를 실행하는 스레드를 메인 스레드라고 한다.
	cout << "메인 스레드 시작" << endl;

	thread t = thread{ Wait };			// 스레드 안에서 새로운 스레드를 생성할 수 있으며, 실행할 메서드의 이름을 전달해야 한다.
										// 생성된 스레드는 설정한 함수만을 연산한다.
										// C#은 서브 스레드를 foreground, background 설정할 수 있지만 C나 C++은 background로만 생성 가능

	t.join();							// t가 끝날 때까지 메인 스레드는 대기한다.
										// join을 하지 않으면 메인 스레드가 서브 스레드보다 먼저 종료하게 되므로 오류발생

	cout << "메인 스레드 종료" << endl;
}