#include <iostream>
#include <thread>

using namespace std;

int sum;	// 전역 변수, 공유메모리 모델
			// 모든 스레드가 접근할 수 있고, 값도 변경할 수 있다.

void Thread_Func()	// 스레드에 할당할 함수
{
	for (auto i = 0; i < 25000000; ++i)	// 스레드가 2개라서 2500만번
		sum += 2;						// 전역 변수에 2씩 더하고 저장
}

int main()
{
	thread t1 = thread{ Thread_Func };
	thread t2 = thread{ Thread_Func };

	t1.join();
	t2.join();

	cout << "sum : " << sum << endl;	// 값은 1억이 아니라 5000만 근처의 값을 보여준다.
}

// 문제는 sum += 2;에 있다.
// 읽기, 연산, 쓰기 3개 작업이 한 줄에 다 있다.
// 여러 작업이기 때문에 연산 도중 다른 스레드가 읽을 수 있고, 읽기 도중 다른 스레드가 쓰기를 하는 경우가 있다.
// 이를 경쟁상태(DataRace)라고 한다.
// 경쟁상태의 정의
// 1. 하나의 데이터를 여러 데이터가 접근
// 2. 적어도 하나의 스레드는 쓰기를 하는 상황