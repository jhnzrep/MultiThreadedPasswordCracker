using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using PasswordCrackerClient.models;
using Newtonsoft.Json;
using System.Diagnostics;
using PasswordCrackerClient.Util;

namespace PasswordCrackerServer
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

        private List<Chunk> chunks;

        public List<Chunk> Chunks
        {
            get { return chunks; }
            set { chunks = value; }
        }

        private bool Finished;

        public TcpWorker(int port, IPAddress ip, List<Chunk> chunkList)
        {
            Port = port;
            Ip = ip;
            Chunks = chunkList;
            results = new List<UserInfoClearText>();
            Finished = false;
        }

        List<UserInfoClearText> results;

        public void Start()
        {
            List<UserInfo> userInfos =
                PasswordFileHandler.ReadPasswordFile("passwords.txt");

            TcpListener tcpListener = new TcpListener(Ip,9999);
            tcpListener.Start();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (Finished == false)
            {
                Task.Run(() =>
                {
                    TcpClient cl = tcpListener.AcceptTcpClient();
                    var resultFromClinet = ClientWork(cl);
                    results.AddRange(resultFromClinet);
                    //Thread.Sleep(10000);
                });
            }
            stopWatch.Stop();
            Console.WriteLine("Time elapsed since server start: " + stopWatch.Elapsed.ToString());
            Console.WriteLine("{0} passwords found out of {1}", results.Count, userInfos.Count);
            foreach (var user in results)
            {
                Console.WriteLine(user.UserName + " : " + user.Password);
            }
        }



         List<UserInfoClearText> ClientWork(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            writer.AutoFlush = true;
            List<UserInfoClearText> answer = new List<UserInfoClearText>();

            foreach (var item in Chunks)
            {
                if (item.Sent == false)
                {
                    item.Sent = true;
                    foreach (var word in item.WordList)
                    {
                        writer.WriteLine(word);
                    }
                    writer.WriteLine("END");
                    var resultsFromClient = reader.ReadLine();
                    answer = JsonConvert.DeserializeObject<List<UserInfoClearText>>(resultsFromClient);
                    if (item == Chunks[Chunks.Count - 1]) Finished = true;
                    break;
                }
            }
            return answer;
            /*Task.Run(() => {
                while (true)
                {
                    try
                    {
                        string str = reader.ReadLine();
                        for (int i = 65; i <= 122;i++)
                        {
                            if (str.Contains(Convert.ToChar(i)))
                            {
                                Console.WriteLine(str);
                                Thread.Sleep(1000);
                            }
                        }
                       
                    }
                    catch
                    {

                    }
                }
            });*/



        }
    }
}
