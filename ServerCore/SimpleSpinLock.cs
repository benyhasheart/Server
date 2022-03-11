using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class SpinLock
    {
        volatile int _lock = 0;

        //#region WrongSpinLock
        //public void Acquire()
        //{
        //    while(true)
        //    {
        //        // 다른 스레드와 동시에 접근해서 권한을 얻어 갈 확률이 높음.
        //        // CAS compare-and-swap 을 사용해야 한다.
        //        if(_lock == 0)
        //        {
        //            break;
        //        }
        //        _lock = 1;
        //    }
        //}
        //#endregion WrongSpinLock

        public void Acquire()
        {
            while (true)
            {
                int expected = 0;
                int desired = 1;
                // CAS compare-and-swap 을 사용
                // expected 상태가 되면 _lock을 desired에 대입한다. 비교와 swap을 다 했다.
                if (Interlocked.CompareExchange(ref _lock, desired, expected) == expected)
                {
                    break;
                }
                
            }
        }
        public void Release()
        {
            _lock = 0;
        }
    }

    class SimpleSpinLock
    {
        static SpinLock _lock = new SpinLock();
        static int _num = 0;

        static void Thread1()
        {
            _lock.Acquire();
            for (int i = 0; i < 100000; i++)
            {
                _num++;
            }
            _lock.Release();
        }

        static void Thread2()
        {
            _lock.Acquire();
            for (int i = 0; i < 100000; i++)
            {
                _num--;
            }
            _lock.Release();
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread1);
            Task t2 = new Task(Thread2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
}
