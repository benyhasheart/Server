using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAcceptHandler;
        public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler )
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;
            //소켓에 ip 바인드
            _listenSocket.Bind(endPoint);

            //10명까지 대기자를 받을 수 있음.
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompeleted);
            RegisterAccept(args);
        }
        //비동기 accept
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);

            if ( pending == false)
            {
                OnAcceptCompeleted(null, args);
            }
        }

        void OnAcceptCompeleted(object sender, SocketAsyncEventArgs args)
        {
            if ( args.SocketError == SocketError.Success)
            {
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);
        }
        public Socket Accept()
        {
            return _listenSocket.Accept();
        }
    }
}
