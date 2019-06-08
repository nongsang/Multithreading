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
        static int sum1;
        static int sum2;

        public static void Main()
        {
            List<Thread> worker_threads = new List<Thread>();
            Stopwatch sw = new Stopwatch();

            for (var i = 1; i <= 32; i += i)
            {
                sum1 = 0;
                sum2 = 0;
                worker_threads.Clear();

                sw.Restart();

                for (var j = 0; j < i; ++j)
                {
                    worker_threads.Add(new Thread(_ => Thread_Func1(i)));
                    worker_threads.Add(new Thread(_ => Thread_Func2(i)));
                    worker_threads.Add(new Thread(_ => Thread_Func3(i)));
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

                Write("threads : {0} / sum1 : {1} / sum2 : {2} / ", i, sum1, sum2);
                WriteLine("time : {0} s", (double)sw.ElapsedTicks / Stopwatch.Frequency);
            }
        }

        static void Thread_Func1(int num_threads)
        {
            for (var i = 0; i < 100000 / num_threads; ++i)
            {
                Interlocked.Add(ref sum1, 1);
            }
        }

        static void Thread_Func2(int num_threads)
        {
            for (var i = 0; i < 100000 / num_threads; ++i)
            {
                Interlocked.Add(ref sum2, 1);
            }
        }

        static void Thread_Func3(int num_threads)
        {
            for (var i = 0; i < 100000 / num_threads; ++i)
            {
                Interlocked.Add(ref sum1, sum2);	    // <atomic> + <atomic>
            }
        }
    }
}

// 아토믹으로 선언된 두 변수끼리 연산을 하면 결과는 올바르게 나오지 않는다.
// <atomic> + <atomic> != <atomic> 으로 결론이 나온다.
// 그렇다면 어떻게 해야 성능을 올리면서 올바른 결과값을 나오게 할 수 있을까?