using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;       // 스레드 관련 기능을 쓰려면 추가

namespace CPPPP
{
    class Thread_Create
    {
        public static void Main()           // Main() 메서드를 실행하기 위해서 스레드 1개는 무조건 사용하게 된다.
        {                                   // Main() 메서드를 실행하는 스레드를 메인 스레드라고 한다.
            WriteLine("메인 스레드 시작");

            Thread t = new Thread(Wait);    // 스레드 안에서 새로운 스레드를 생성할 수 있으며, 실행할 메서드의 이름을 전달해야 한다.
                                            // 생성된 스레드는 설정한 메서드만을 연산한다.
            t.IsBackground = true;  // 스레드가 포그라운드로 실행될지, 백그라운드로 실행될지 결정한다.
                                    // 기본 값은 false로 포그라운드로 실행된다.
                                    // 포그라운드는 메인 스레드가 종료되도 서브 스레드가 종료될 때 까지 프로그램이 살아있게 된다.
                                    // 백그라운드는 메인 스레드가 종료되면 서브 스레드는 자동으로 중단된다.
                                    // C나 C++은 백그라운드만 실행이 된다.

            t.Start();  // t 스레드를 시작

            t.Join();   // t가 끝날 때까지 메인 스레드는 대기한다.
                        // 조인을 하지 않으면 메인 스레드가 서브 스레드보다 먼저 종료하는 경우가 발생한다.
                        // 서브 스레드들이 포그라운드, 백그라운드인지 상관없이 조인을 하면 메인 스레드는 Join을 설정한 스레드를 기다린다.
                        // 반드시 조인을 하라는 규칙은 없다.
                        // 조인을 하지 않으면 포그라운드, 백그라운드의 설정에 따라 동작한다.
                        // 메인 스레드가 서브 스레드의 결과값을 명시적으로 얻야 하는 경우에만 사용해도 무방하다.
                        // C나 C++은 포그라운드로 실행할 수 없으므로 조인을 반드시 해줘야 한다.

            WriteLine("메인 스레드 종료");
        }

        static void Wait()
        {
            WriteLine("서브 스레드 시작");

            Thread.Sleep(1000);             // 1초간 대기

            WriteLine("서브 스레드 시작");
        }
    }
}