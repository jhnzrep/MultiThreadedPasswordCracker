using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PasswordCrackerServer
{
    class ChunkMaker
    {


        public static List<Chunk> CreateChunks(int number)
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

            for (int j = 0; j <= number - 1; j++)
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
        }

        public static List<Chunk> CreateChunks(List<string> fullDicionary)
        {
            int maxi = fullDicionary.Count / 4;

            List<Chunk> chunkList = new List<Chunk>();

            for (int j = 0; j <= 4 - 1; j++)
            {
                int initiali = maxi - fullDicionary.Count / 4;
                List<string> list1 = new List<string>();
                for (int i = initiali; i < maxi; i++)
                {
                    list1.Add(fullDicionary[i]);
                }
                chunkList.Add(new Chunk(list1));

                maxi += fullDicionary.Count / 4;
            }

            return chunkList;
        }
    }
}
