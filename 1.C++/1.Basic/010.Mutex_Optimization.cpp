#include <iostream>
#include <thread>
#include <chrono>
#include <vector>
#include <atomic>
//#include <mutex>

using namespace std;
using namespace std::chrono;

atomic<int> sum;	// 변수 한개에 대한 연산(덧셈, 뺄셈같은 간단한 연산)을 아토믹하게 하고 싶으면 아토믹변수 사용
//mutex mylock;		// 여러 작업(함수와 같은 복잡한 연산이 여러개)을 아토믹하게 하고 싶으면 상호배재 사용

void Optimal_Thread_Func(int num_threads)
{
	volatile int local_num = 0;	// volatile은 최적화를 하지 않아서 메모리에서 값을 읽어오고 처리한 다음 저장한다.
								// 최적화를 하면 바로 결과값을 구할 수 있고 더 빠르지만, 스레드의 갯수에 따라 속도가 빨라지는지 확인하기 위해 사용

	for( auto i = 0; i < 500000000 / num_threads; ++i )
	{
		local_num += 2;	// volatile을 사용했으므로 2, 4, 6 ... 과정을 그대로 수행한다.
						// volatile을 사용하지 않으면 자동 최적화를 하여 1억이 local_num에 바로 저장이 되서 속도가 빨라지는지 확인하기 힘들다.
	}
	
	//mylock.lock();	// sum이 그냥 int형이면 lock을 사용하면 된다.
	sum += local_num;	// sum이 atomic<int>형이면 그냥 더해도 된다.
	//mylock.unlock();	// 가장 중요한 점은 각자 작업을 처리하고 결과를 낼 때 순서를 지키게 설계를 하면 된다는 것이다.
						// 각자 작업마다 lock을 거는 것보다 막판에만 lock을 걸면 속도도 올리고 결과값도 옳게 나온다.
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

		cout << "time : " << static_cast<double>(duration_cast<microseconds>(du).count()) / 1000000 << " μs\n";
	}
}