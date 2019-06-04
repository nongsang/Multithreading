using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;         // 스레드 관련 기능을 쓰려면 추가

namespace CPPPP
{
    class Parallel_Basic
    {
        public static void Main()
        {
            WriteLine("피보나치 수열 구하기 (종료 'x')");

            while(true)
            {
                Write("숫자를 입력하세요. : ");
                string input = ReadLine();      // 문자열 입력

                if(input.Equals("x", StringComparison.OrdinalIgnoreCase))   // x가 입력되면 프로그램 종료
                {                                                           // 스레드가 foreground여도 break로 종료되면 서브 스레드들도 강제로 종료
                    WriteLine("프로그램 종료");
                    break;
                }

                //WriteLine("{0}", Fib(int.Parse(input)));    // 기본적으로 메서드는 동기방식으로 동작한다.
                                                              // 오래 걸리면 x를 입력해서 탈출해야 하지만 메서드가 실행되는 동안 다른 작업이 불가능하다.
                                                              // 동기방식의 한계
                Thread t = new Thread(_ => WriteLine(Fib(int.Parse(input))));   // 서브 스레드를 생성하여 연산을 맡긴다.
                                                                                // 메인 스레드는 서브 스레드를 생성하고 x입력만 들어오기만을 기다린다.
                                                                                // while문 안에 스레드 생성이 있다고 계속 생성하는 것이 아니다.
                                                                                // 여전히 동기방식이지만 스레드를 생성해서 전담하는 방식으로 동기방식의 문제를 해결했다.
                                                                                // A 스레드, B 스레드 나름대로 동시에 일을 시키는 것이기 때문에 병렬(Parallel)처리를 한 것이다.
                t.IsBackground = true;
                t.Start();
            }

            //t.Join();   // 여기서는 Join을 하지 않았다.
                          // 1. t는 while문 안에서 생성했기 때문에 while문 밖에서 t에 접근을 할 수 없다.
                          // 2. t는 백그라운드로 실행이 되기 때문에 Join을 하지 않아도 메인 스레드가 종료되면 자동으로 종료된다.
                          // 조인을 사용하도록 설계한다면 while문 밖에서 스레드를 생성해야 한다.
        }

        static int Fib(int n)   // 피보나치 수열, 입력값이 커지면 기하급수적으로 성능이 하락한다.
        {
            if (0 == n) return 0;
            else if (1 == n) return 1;
            else return Fib(n - 1) + Fib(n - 2);
        }
    }
}