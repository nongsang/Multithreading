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
            for (var i = 0; i < 50000000 / num_threads; ++i)
            {
                Interlocked.Add(ref sum, 2);    // Interlocked 정적 클래스를 이용한 아토믹 연산
            }
        }
    }
}

// 아토믹이란
// 1. 모든 Atomic 연산은 다른 Atomic 연산과 동시에 수행되지 않는다.(상호배재)
// 2. Atomic 연산은 이전의 명령어의 실행이 모두 끝날 때 까지 시작되지 않는다.
// 3. 모든 Atomic 연산의 실행 순서는 모든 쓰레드에서 동일하게 관찰된다.

// 아토믹은 자칫 만능으로 생각할 수 있다.
// 아토믹을 사용해도 문제는 여전히 발생한다.