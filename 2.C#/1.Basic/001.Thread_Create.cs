﻿using System;
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
                        // 조인을 하지 않으면 메인 스레드가 서브 스레드보다 먼저 종료하는 경우가 발생할 수 있다.
                        // 서브 스레드들이 포그라운드, 백그라운드인지 상관없이 조인을 하면 메인 스레드는 조인을 설정한 스레드를 기다린다.
                        // 반드시 조인을 하라는 규칙은 없다.
                        // 조인을 하지 않으면 포그라운드, 백그라운드의 설정에 따라 동작한다.
                        // 조인을 한다는 의미는 컴퓨터 의미로 스레드 변수 t가 실행중인 서브 스레드를 가리키고 있다는 의미다.
                        // 메인 스레드에서 서브 스레드의 연산 결과값이 필요하다는 의도에서다.
                        // 연산 결과값이 필요없다면 조인을 하지 않아도 되며, 스레드 변수가 서브 스레드를 가리키지 않게 하면 된다.
                        // C#은 스레드 변수가 실행중인 서브 스레드를 가리키고 있을 때 조인을 하지 않아도 오류가 없다.
                        // C나 C++은 백그라운드로만 생성 가능하다.
                        // 게다가 스레드 변수가 실행중인 서브 스레드를 가리키고 있을 때 조인을 하지 않으면 오류가 발생한다.

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