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
                {                                                           // 서브 스레드들이 백그라운드로 설정이 되어있으면 모두 강제로 종료
                    WriteLine("프로그램 종료");
                    break;
                }

                //WriteLine("{0}", Fib(int.Parse(input)));    // 기본적으로 메서드는 동기방식으로 동작한다.
                                                              // 오래 걸리면 x를 입력해서 탈출해야 하지만 메서드가 실행되는 동안 다른 작업이 불가능하다.
                                                              // 동기방식의 한계
                Thread t = new Thread(_ => WriteLine(Fib(int.Parse(input))));   // 서브 스레드를 생성하여 연산을 맡긴다.
                                                                                // 메인 스레드는 서브 스레드를 생성하고 다시 입력을 기다린다.
                                                                                // 서브 스레드가 연산을 하고 있어도 입력을 받을 때 마다 서브 스레드를 생성하여 연산을 맡긴다.
                                                                                // 여전히 동기방식이지만 스레드를 생성해서 전담하는 방식으로 동기방식의 문제를 해결했다.
                                                                                // 메인 스레드, 서브 스레드 각자 나름대로 동시에 일을 하는 것이기 때문에 병렬(Parallel)처리를 한 것이다.
                t.IsBackground = true;
                t.Start();

                //new Thread(_ => WriteLine(Fib(int.Parse(input))));  // 람다는 Fib()의 결과값을 출력하는 역할만 한다.
                                                                      // 메인 스레드는 서브 스레드의 결과값이 필요없다는 의미다.
                                                                      // 따라서 조인을 할 이유가 없으므로 익명 스레드를 생성해서 연산을 해도 된다.
                                                                      // 하지만 익명 스레드는 무조건 포그라운드로 실행된다.
                                                                      // 메인 스레드가 종료되면 서브 스레드도 종료되게 해야 하기 때문에 익명 스레드는 적절하지 않다.
            }

            //t.Join();   // 여기서는 조인을 하지 않았다.
                          // 이유는
                          // 1. t는 while문 안에서 생성했기 때문에 while문 밖에서 t에 접근을 할 수 없다. (스택프레임문제)
                          // 2. t의 스레드 연산 결과는 메인 스레드가 필요로 하지 않다. (설계)
                          // 3. 메인 스레드가 종료하면 서브 스레드도 종료하게 하고 싶다. (스레드 변수 연결 X)
        }

        static int Fib(int n)   // 피보나치 수열, 입력값이 커지면 기하급수적으로 성능이 하락한다.
        {
            if (0 == n) return 0;
            else if (1 == n) return 1;
            else return Fib(n - 1) + Fib(n - 2);
        }
    }
}