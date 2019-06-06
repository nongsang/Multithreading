using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;       // 스레드 관련 기능을 쓰려면 추가

class ClassThreadParam  // 스레드를 생성할 때 전달할 클래스
{
    public int value1;
    public int value2;

    public ClassThreadParam(int a, int b)   // 생성자
    {
        value1 = a;
        value2 = b;
    }
}

struct StructThreadParam    // 스레드를 생성할 때 전달할 구조체
{
    public int value1;
    public int value2;

    public StructThreadParam(int a, int b)
    {
        value1 = a;
        value2 = b;
    }
}

class Adder // C++의 펑터를 흉내낸 클래스
{
    private int value1;
    private int value2;

    public Adder(int a, int b)
    {
        value1 = a;
        value2 = b;
    }

    //public static override void operator() () {}    // C#에서도 펑터를 정의할 수 있지만 호출 연산자를 재정의 할 수 없다.
    // 따라서 오로지 메서드의 모음집 같은 개념으로 스레드 생성은 불가능하다.
    public void String()           // 호출 연산자 개념으로 사용하는 메서드
    {
        Write("Adder(int, int).string() : ");
        WriteLine((value1 + value2).ToString());
    }
}

namespace CPPPP
{
    class Thread_Parameter
    {
        public static void Main()
        {
            Thread t1 = new Thread(Calc);   // 매개변수가 없는 메서드는 스레드를 생성할 때 생성자에 메서드 이름을 전달하면 된다.
            t1.IsBackground = true;
            t1.Start();                     // 스레드 시작

            Thread t2 = new Thread(new ThreadStart(Calc));  // Thread 생성자로 델리게이트인 ThreadStart를 생성하여 전달해주게끔 정의가 되어있다.
                                                            // t1처럼 ThreadStart를 생략해도 자동으로 처리해준다.
            t2.IsBackground = true;
            t2.Start();                                     // 스레드 시작

            Thread t3 = new Thread(Add);            // 매개변수가 있는 메서드도 델리게이트로 전달하기 때문에 메서드 이름을 전달하면 된다.
                                                    // 하지만 매개변수가 object형이면서 1개만 있는 메서드만 전달할 수 있다.
                                                    // 왜냐하면 매개변수가 있는 델리게이트는 object형 매개변수 1개만 있는 델리게이트만 정의를 해놨기 때문이다.
            t3.IsBackground = true;
            t3.Start(new ClassThreadParam(1, 2));   // 때문에 Start()도 매개변수가 object형 매개변수 1개만 받을 수 있게 정의가 되어있다.
                                                    // Start()에 넣은 매개변수는 생성한 스레드와 연결된 메서드에 매개변수로 전달되어 연산을 한다.
                                                    // 매개변수 2개 이상 사용하고 싶으면 클래스형을 정의해서 전달해줘야 한다.


            Thread t4 = new Thread(new ParameterizedThreadStart(Sub));  // 매개변수가 있는 메서드를 전달할 델리게이트는 ParameterizedThreadStart로 정의되어 있다.
                                                                        // 다만 매개변수가 object형 1개로만 정의되어있다.
            t4.IsBackground = true;
            t4.Start(new StructThreadParam(3, 4));                      // 매개변수를 2개 이상 사용하고 싶으면 구조체로도 전달할 수 있다.

            Thread t5 = new Thread(() => { WriteLine("Calc()"); });     // 델리게이트의 축약형인 람다를 생성자에 전달할 수 있다.
            t5.IsBackground = true;
            t5.Start();

            Thread t6 = new Thread(() => { Calc(); });      // 람다 내부에서 메서드를 호출하게 구현할 수 있다.
            t6.IsBackground = true;
            t6.Start();

            Thread t7 = new Thread(() => CircleArea(3));    // 람다 내부에서 메서드를 호출하게 구현하면 매개변수에 직접 값을 대입해 호출하면 된다.
                                                            // C#에서는 람다를 정의할 때 중괄호를 삭제할 수 있어서 더 축약해서 사용할 수 있다.
            t7.IsBackground = true;
            t7.Start();                                     // 그렇기 때문에 Start()에 어떤 매개변수도 전달하지 않아도 된다.

            Thread t8 = new Thread(_ => Mul(5, 6));         // 매개변수가 2개 이상인 메서드도 람다로 전달하면 된다.
                                                            // 람다 내부에서 사용할 외부 변수가 없으면 소괄호 대신 _로 매개변수를 사용하지 않는 람다임을 표시할 수 있다.
            t8.IsBackground = true;
            t8.Start();

            Thread t9 = new Thread(_ => Add(new ClassThreadParam(7, 8)));  // 클래스를 전달해도 되지만 람다로 구현 안하는게 더 좋다.
                                                                           // 클래스를 매개변수로 받아서 처리할 메서드를 따로 구현하기도 귀찮고,
                                                                           // 이미 object형을 받는 델리게이트가 정의가 되어 있기 때문이다.
            t9.IsBackground = true;
            t9.Start();

            Thread t10 = new Thread(_ => Sub(new StructThreadParam(9, 10)));    // 클래스와 마찬가지로 구조체로 전달해도 되지만 람다로 구현하지 않는게 더 좋다.
                                                                                // 구조체를 매개변수로 받아서 처리할 메서드를 따로 구현하기도 귀찮고,
                                                                                // 구조체를 사용하는 이유는 값형식을 한번에 처리한다는게 장점이기 때문에,
                                                                                // 4개 이상의 매개변수를 다루는거 아니면 따로 구조체로 만들어서 전달하는 것보다 하나하나 다루는 것이 편하기 때문
            t10.IsBackground = true;
            t10.Start();

            new Thread(_ => Calc()).Start();   // 익명으로 스레드를 생성하여 실행할 수 있다.
                                               // 다만 익명이기 때문에 스레드를 따로 설정할 수 없고, 생성하자마자 실행되기 때문에 무조건 백그라운드로 실행된다.
                                               // 메인 스레드가 종료되면 자동으로 종료된다.
                                               // 그 이외에는 메인 스레드와 독립적인 연산을 한다는 의미이며, 그 결과값도 메인 스레드에 전달할 필요가 없다.
                                               // 따라서 익명 스레드는 생성하면 조인을 하지 않아도 되는 상황에 사용하는 것이 좋다.

            Thread t11 = new Thread(new Adder(11, 12).String);   // 억지로 C++의 펑터를 구현할 수 있다.
                                                                 // 그런데 굳이 이렇게 써야 하는가?
            t11.IsBackground = true;
            t11.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            t6.Join();
            t7.Join();
            t8.Join();
            t9.Join();
            t10.Join();
            t11.Join();
        }

        static void Calc()
        {
            WriteLine("Calc()");
        }

        static void Add(object o)                           // 매개변수를 가진 델리게이트에 저장하기 위해 object형 1개만 가진 메서드를 정의할 수 밖에 없다.
        {
            Write("Add(object) : ");
            ClassThreadParam param = o as ClassThreadParam; // object형은 강제형변환보다 as로 형변환이 가능한지 확인하고 변환이 되면 저장하는 방식이 좋다.
            WriteLine(param.value1 + param.value2);         // 클래스의 필드끼리 더한 결과값을 전달한다.
        }
        static void Sub(object o)                           // object형 1개만 매개변수로 가진 메서드
        {
            Write("Sub(object) : ");
            StructThreadParam param = (StructThreadParam)o; // 구조체는 null값을 가질 수 없으므로 as를 사용할 수 없기 때문에 강제형변환을 한다.
                                                            // as를 사용하려면 StructThreadParam?형으로 nullable을 사용해야 한다.
            WriteLine(param.value1 - param.value2);
        }

        static void CircleArea(double d)    // 사실 람다를 사용하면 object형이 아니여도 사용가능하다.
        {
            Write("CircleArea(double) : ");
            WriteLine(d * d * 3.14);
        }

        static void Mul(int a, int b)       // 물론 매개변수가 2개 이상도 처리가 가능하다.
        {
            Write("Mul(int, int) : ");
            WriteLine(a * b);
        }
    }
}