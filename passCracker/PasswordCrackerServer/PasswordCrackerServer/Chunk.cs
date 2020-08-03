using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordCrackerServer
{
    class Chunk
    {
        private List<string> wordList;

        private bool sent;

        public bool Sent
        {
            get { return sent; }
            set { sent = value; }
        }



        public List<string> WordList
        {
            get { return wordList; }
            set { wordList = value; }
        }

        public Chunk(List<string> wordList)
        {
            Sent = false;
            WordList = wordList ?? throw new ArgumentNullException(nameof(wordList));
        }
    }
}
