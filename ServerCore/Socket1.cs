using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ServerCore
{

    class Socket1
    {
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
             

                Session session = new Session();
                session.Start(clientSocket);

                // 보내는 데이터
                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");

                session.Send(sendBuff);

                Thread.Sleep(1000);

                session.Disconnect();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        static void Main(string[] args)
        {
            //로컬 호스트 네임
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);



            _listener.Init(endPoint, OnAcceptHandler);
            Console.WriteLine("Listening...");
            while (true)
            {
                ;
            }


        }

    }
}
