#include <iostream>
#include <thread>

using namespace std;

int sum;	// 전역 변수, 공유메모리 모델
			// 모든 스레드가 접근할 수 있고, 값도 변경할 수 있다.

void Thread_Func()	// 스레드에 할당할 함수
{
	for (auto i = 0; i < 25000000; ++i)	// 2500만번
		sum += 2;						// 전역 변수에 2씩 더하고 저장
}

int main()
{
	thread t1 = thread{ Thread_Func };
	thread t2 = thread{ Thread_Func };

	t1.join();		
	cout << "sum : " << sum << endl;	// 값은 1억이 아니라 5000만 근처의 값을 보여준다.
										// 왜냐하면 t1 스레드가 값을 바꾸는 도중 t2가 아직 바뀌지 않은 값을 복사해가고,
										// t2가 값을 바꾸는 도중 t1 스레드가 값을 바꾸는 등 읽고 쓰는 순서에 따라 결과가 달라진다.
										// 이를 경쟁상태(DataRace)라고 한다.
										// 정의) 하나의 데이터를 여러 데이터가 접근하며, 적어도 하나의 스레드는 쓰기를 하는 상황이면 경쟁상테(DataRace)라고 한다.
}