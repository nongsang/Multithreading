using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;         // 스레드 관련 기능을 쓰려면 추가

namespace CPPPP
{
    class Data_Race
    {
        static int sum = 0;     // 전역 필드, 공유메모리 모델
                                // 모든 스레드가 접근할 수 있고, 값도 변경할 수 있다.

        public static void Main()
        {
            Thread t1 = new Thread(ThreadFunc);
            Thread t2 = new Thread(ThreadFunc);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            WriteLine(sum); // 값은 1억이 아니라 5000만 근처의 값을 보여준다.
                            // 왜냐하면 t1 스레드가 값을 바꾸는 도중 t2가 아직 바뀌지 않은 값을 복사해가고,
                            // t2가 값을 바꾸는 도중 t1 스레드가 값을 바꾸는 등 읽고 쓰는 순서에 따라 결과가 달라진다.
                            // 이를 DataRace라고 한다.
                            // 정의) 하나의 데이터를 여러 데이터가 접근하며, 적어도 하나의 스레드는 Write를 하는 상황이면 DataRace라고 한다.
        }

        static void ThreadFunc()                // 스레드에 할당할 메서드
        {
            for (int i = 0; i < 25000000; ++i)  // 2500만번
                sum += 2;                       // 전역 필드에 2씩 더하고 저장
        }
    }
}