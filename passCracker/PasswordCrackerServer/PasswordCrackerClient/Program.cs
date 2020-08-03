using System;
using System.Net.Sockets;
using System.Net;
namespace PasswordCrackerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpWorker tcpWorker = new TcpWorker(9999, IPAddress.Loopback);
            tcpWorker.Start();
        }
    }
}
