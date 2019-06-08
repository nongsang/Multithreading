using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;       // 스레드 관련 기능을 쓰려면 추가
using System.Diagnostics;

namespace CPPPP
{
    class Thread_Parameter
    {
        static int sum;

        static Mutex mylock = new Mutex();  // 상호배재

        public static void Main()
        {
            List<Thread> worker_threads = new List<Thread>();   // 자작 스레드풀 생성
            Stopwatch sw = new Stopwatch();

            for (var i = 1; i <= 32; i += i)   // 스레드가 1개 ~ 32개일 때 성능을 측정
            {
                sum = 0;
                worker_threads.Clear();

                sw.Restart();  // 시간 측정 시작

                for (var j = 0; j < i; ++j)                                 // 스레드 갯수만큼
                {
                    worker_threads.Add(new Thread(_ => Thread_Func(i)));    // 스레드를 생성하여 스레드풀에 저장
                }

                foreach (var t in worker_threads)   // 스레드풀에 있는 모든 스레드
                {
                    t.Start();                      // 시작
                }

                foreach (var t in worker_threads)   // 스레드풀에 있는 모든 스레드
                {
                    //t.Start();                    // 이 위치에서 스레드 시작하면 안된다.
                                                    // 왜냐하면 서브 스레드 1개만 생성했지만 바로 다음 명령인 조인이 있으므로
                                                    // 메인 스레드는 강제로 1개만 생성된 서브 스레드를 기다려야 한다.
                    t.Join();                       // 대기
                }

                sw.Stop();  // 시간 측정 끝

                Write("threads : {0} / sum : {1} / ", i, sum);                              // 결과 출력
                WriteLine("time : {0} s", (double)sw.ElapsedTicks / Stopwatch.Frequency);   // 시간 출력
            }
        }

        static void Thread_Func(int num_threads)
        {
            for (var i = 0; i < 500000 / num_threads; ++i)  // 너무 속도가 나오지 않은 나머지 50만번만 반복한다.
            {
                mylock.WaitOne();   // 한번에 한개의 스레드만 접근할 수 있게 잠금
                                    // 하나의 스레드만 잠금을 얻고 다음 작업을 할 수 있다.
                                    // 나머지 스레드들은 큐(Queue)에 저장해 놓고 스핀하며 대기

                sum += 2;           // 읽기, 연산, 쓰기 3개 작업이 한 줄에 다 있다.
                                    // 여러 작업이기 때문에 연산 도중 다른 스레드가 읽을 수 있고, 읽기 도중 다른 스레드가 쓰기를 하는 경우가 있다.
                                    // 때문에 한번에 한 스레드만 접근해서 작업을 하게 하면 올바른 결과가 나올 것이다.

                mylock.ReleaseMutex();  // 작업을 끝낸 스레드는 잠금을 푼다.
                                        // 큐에 있는 스레드가 다음 잠금을 얻고 작업을 한다.
            }
        }
    }
}
// 스레드를 많이 만들어서 작업을 시켜서 성능이 올라가길 기대했다.
// 하지만 스레드가 많을 수록 속도는 더 떨어진다.
// 상호배재가 문제가 된다.
// 락을 얻은 스레드 1개만 연산을 하게 되는데, 그러면 싱글스레드로 연산을 하는 것과 차이가 없어진다.
// 스레드가 많아지면 스레드들이 번갈아가면서 연산을 하기 때문에 다른 스레드에게 연산 권한을 넘겨주는 교환작업도 추가가 된다.
// 이 스레드 교환 작업을 문맥교환(Context Switching)이라고 한다.
// 상호배재로 싱글스레드와 다를바 없는 작업이 되었고, 스레드가 많아질 수록 문맥교환은 많아지기 때문에 성능은 더욱 떨어질 수 밖에 없다.