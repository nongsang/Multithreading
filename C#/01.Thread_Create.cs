using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;    // WriteLine()이라고 줄여서 쓰고싶어
using System.Threading;       // 스레드 관련 기능을 쓰려면 추가

namespace CPPPP
{
    class Thread_Create
    {
        public static void Main()           // Main() 메소드를 실행하기 위해서 이미 스레드 1개는 무조건 사용하게 된다.
        {                                   // Main() 메소드를 실행하는 스레드는 주된 스레드이므로 주 스레드라고 한다.
            WriteLine("주 스레드 시작");

            Thread t = new Thread(Wait);    // 스레드 안에서 새로운 스레드를 생성할 수 있으며, 메소드의 이름을 전달해야 한다.

            t.Start();      // t스레드를 시작

            t.Join();       // t가 끝날 때까지 메인 스레드는 대기한다.
                            // 이 문장이 없으면 t는 실행하고 있는데 주 스레드가 먼저 끝날 가능성 있다.
                            // t스레드는 주 스레드에서 생성이 됬기 때문에 주 스레드가 끝나고 삭제되면 t는 작업도중 일이 중단되어 오류를 일으킨다.
                            // C#에서는 주 스레드가 끝났다고 다른 스레드에서 오류를 일으키진 않으나 C나 C++에서는 오류가 발생하므로 꼭 확인하자.
                            // 따라서 주 스레드의 작업이 끝나더라도 t의 작업이 끝나고 난 후에 주 스레드를 삭제하게끔 join을 해야한다.
            WriteLine("주 스레드 종료");
        }

        static void Wait()
        {
            WriteLine("스레드 시작");
            Thread.Sleep(3000);     // Sleep은 스레드를 일정시간 멈춘다.
            WriteLine("스레드 종료");// ms 기준으로 입력을 받으므로 3000은 3초를 의미
        }
    }
}