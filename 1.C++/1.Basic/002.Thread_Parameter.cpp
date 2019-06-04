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

class Adder	// 펑터
{
private:
	int a;
	int b;
public:
	Adder(int _a, int _b) : a(_a), b(_b) {}	// 생성자
	void operator()() const					// 형변환 연산자
	{
		cout << "Adder.operator() : " << a + b << endl;
	}
};

int main()
{
	thread t1 = thread{ Calc };	// 매개변수가 없는 함수는 스레드를 생성할 때 매겨변수로 함수 이름을 전달하면 된다.

	void (*func)() = Calc;		// 함수포인터 생성
	thread t2 = thread{ func };	// thread 생성자로 함수포인터를 전달해준다.
								// 기본적으로 스레드 생성자의 매개변수가 함수포인터로 정의가 되어 있음을 알 수 있다.

	thread * t3 = new thread{ func };	// 동적으로 스레드를 생성하여 사용할 수 있다.

	thread t4 = thread{ Add, 1, 2 };	// C#과 달리 매개변수를 2개 이상 전달하여 스레드를 생성할 수 있다.

	thread t5 = thread{ Adder(3, 4) };	// 펑터로 스레드 생성 가능

	thread t6 = thread{ []() { cout << 5 - 6 << endl; } };	// 람다로 스레드 생성 가능

	thread t7 = thread{ []() { Sub(7, 8); } };	// 람다 내부에서 함수 호출로 스레드 생성 가능

	thread t8 = thread{ [] { Add(9, 10); } };	// 람다를 함수포인터에 연결하지 않고 바로 사용하기 때문에 매개변수가 없어도 되지 때문에
												// 소괄호를 생략해서 람다를 호출할 수 있다.
	
	new thread([] { Add(11, 12); });	// new를 사용하여 익명으로 스레드를 실행할 수 있다.
										// 메인 스레드에서 생성한 서브 스레드이므로 메인 스레드가 종료되면 자동으로 종료된다.
										// 그 이외에는 메인 스레드와 독립적인 연산을 한다는 의미이며, 그 결과값도 메인 스레드에 전달할 필요가 없다.
										// 따라서 익명 스레드를 생성하면 조인을 하지 않아도 된다.
										// 스레드 변수가 서브 스레드를 가리키지 않는 경우이므로 조인이 없어도 된다.

	t1.join();
	t2.join();
	t3->join();	// t3은 포인터형이므로 사용했으니 접근연산자는 ->
	t4.join();
	t5.join();
	t6.join();
	t7.join();
	t8.join();

	delete(t3);	// t3을 동적으로 생성했으니 메모리 해제도 해줘야 한다.
}

// 실행 결과가 뒤죽박죽이다.
// 병렬로 실행되는 스레드들이 순서에 상관없이 결과물을 내기 때문이다.
// 스레드들이 실행된 순서에 따라 결과물을 내는 것을 기대했을 것이다.
// C#은 스레드를 실행한 순서를 어느정도 지켜서 결과를 내지만 C++은 아니다.