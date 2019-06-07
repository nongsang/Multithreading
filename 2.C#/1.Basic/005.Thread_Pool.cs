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
            for (var i = 0; i < 500000000 / num_threads; ++i)   // 스레드의 갯수만큼 전체 할 일을 나누어서 실행한다.
            {                                                   // 스레드가 많아지면 경쟁상태는 더욱 심해진다.
                sum += 2;
            }
        }
    }
}

// 스레드가 많아질 수록 결과값은 더 이상해진다.
// 경쟁상태가 더 심해지기 때문이다.
// 만약 작업을 하나의 스레드만 접근하게 해서 연산을 하게 하고 나머지 스레드는 대기를 하게하면 결과값이 옳게 나오겠지?