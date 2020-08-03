using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordCrackerServer
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Chunk> chunkList = new List<Chunk>();
            Console.WriteLine("Welcome to Awesome International's Password Cracker!");
            Console.WriteLine("Please enter number of chunks to split dictionary into:");
            chunkList = ChunkMaker.CreateChunks(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Server strated, ready for client's to connect.");

            TcpWorker t = new TcpWorker(9999, IPAddress.Any,chunkList);
            
               
                    t.Start();



            Console.ReadKey();

           
        }

       /* static List<Chunk> CreateChunks(int number)
        {
            List<string> fullDicionary = new List<string>();

            string filename = "webster-dictionary.txt";
            
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fs))
            {

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    fullDicionary.Add(line);
                }
              
            }
            
            int maxi = fullDicionary.Count / number;

            List<Chunk> chunkList = new List<Chunk>();

            for (int j = 0; j <= number-1; j++)
            {

                int initiali = maxi - fullDicionary.Count / number;
                List<string> list1 = new List<string>();
                for (int i = initiali; i < maxi; i++)
                {
                    list1.Add(fullDicionary[i]);
                }
                chunkList.Add(new Chunk(list1));
                
                maxi += fullDicionary.Count / number;
            }

            return chunkList;
        }*/
    }
}
