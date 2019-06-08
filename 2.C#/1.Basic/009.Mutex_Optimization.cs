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

        static Mutex mylock = new Mutex();

        public static void Main()
        {
            List<Thread> worker_threads = new List<Thread>();
            Stopwatch sw = new Stopwatch();

            for (var i = 1; i <= 32; i += i)
            {
                sum = 0;
                worker_threads.Clear();

                sw.Restart();

                for (var j = 0; j < i; ++j)
                {
                    worker_threads.Add(new Thread(_ => Optical_Thread_Func(i)));
                }

                foreach (var t in worker_threads)
                {
                    t.Start();
                }

                foreach (var t in worker_threads)
                {
                    t.Join();
                }

                sw.Stop();

                Write("threads : {0} / sum : {1} / ", i, sum);
                WriteLine("time : {0} s", (double)sw.ElapsedTicks / Stopwatch.Frequency);
            }
        }

        static void Optical_Thread_Func(int num_threads)
        {
            int local_sum = 0;  // 지역변수를 생성해서 내부에서 각자 작업을 시킨다.

            for (var i = 0; i < 50000000 / num_threads; ++i)
            {
                local_sum += 2; // 지역변수에 병렬로 값을 더한다.
            }

            // 최종 결과를 낼 때 순서를 지켜서 연산을 하게 한다.
            // 모든 연산에 상호배재를 하는 것보다 각자 일을 한 후에 마지막 업데이트 할 때만 순서를 지키게끔 상호배재를 한다.
            // 그러면 스레드의 갯수만큼만 잠금이 발생하므로 5000만번 잠금이 발생하는 것보다 빠른 속도를 가지며, 올바른 결과를 가지게 된다.

            //mylock.WaitOne();       // 잠금을 얻어서 순서대로
            //sum += local_sum;       // 각 스레드 결과를 더한다.
            //mylock.ReleaseMutex();

            Interlocked.Add(ref sum, local_sum);    // mutex 대신 아토믹하게 연산해도 된다.
        }
    }
}