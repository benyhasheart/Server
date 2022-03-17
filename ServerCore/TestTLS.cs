using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    //TLS
    //Thread의 고유한 영역

    
    class TestTLS
    {

        static ThreadLocal<String> ThreadName = new ThreadLocal<string>(() => {
            return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; });

        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;

            if (repeat)
            {
                Console.WriteLine(ThreadName.Value + "repeat");
            }
            else
            {
                Console.WriteLine(ThreadName.Value);

            }
            //ThreadName.Value = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";

            //Thread.Sleep(1000);

            //Console.WriteLine(ThreadName);


        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            //ThreadPool에서 준비된 Thread를 사용한다.
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            ThreadName.Dispose();
        }
    }
}
