using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ServerCore
{
    class Socket1
    {
        static void Main(string[] args)
        {
            //로컬 호스트 네임
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //소켓 생성, TCP 전송방식의 소켓
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            try
            {
                //소켓에 ip 바인드
                listenSocket.Bind(endPoint);

                //10명까지 대기자를 받을 수 있음.
                listenSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("Listening...");
                    //연결된 클라이언트
                    Socket clientSocket = listenSocket.Accept();

                    // 받은 데이터
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"[From Client] {recvData}");

                    // 보내는 데이터
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");
                    clientSocket.Send(sendBuff);

                    //내보낸다
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }
        
    }
}
