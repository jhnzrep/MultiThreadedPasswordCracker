using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using PasswordCrackerClient.models;
using Newtonsoft.Json;
using PasswordCrackerServer;
using System.Diagnostics;

namespace PasswordCrackerClient
{
    class TcpWorker
    {
        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private IPAddress ip;

        public IPAddress Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public TcpWorker(int port, IPAddress ip)
        {
            Port = port;
            Ip = ip;
        }


        public void Start()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> wordList = new List<string>();
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            NetworkStream networkStream = tcpClient.GetStream();
            StreamWriter writer = new StreamWriter(networkStream);
            StreamReader reader = new StreamReader(networkStream);
            string str = reader.ReadLine();
            writer.AutoFlush = true;
            while (str!="END")
            {
                str = reader.ReadLine();
                wordList.Add(str);
            }

            List<UserInfoClearText> result = new List<UserInfoClearText>();

            List<Chunk> chunks = ChunkMaker.CreateChunks(wordList);
            var tasks = Task.Run(() =>
            {
                Parallel.For(0, 4, i =>
                {
                    Cracker crack = new Cracker(chunks[i].WordList);
                    result.AddRange(crack.RunCracker());
                });
            });
            tasks.Wait();
            stopwatch.Stop();
            Console.WriteLine(string.Join(", ", result));
            Console.WriteLine("Time elapsed since start " + stopwatch.Elapsed);
            writer.WriteLine(JsonConvert.SerializeObject(result));
            tcpClient.Close();
            Console.ReadKey();




        }



        void ClientWork(TcpClient client)
        {
           

        }
    }
}
