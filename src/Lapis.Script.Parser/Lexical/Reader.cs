/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : Reader
 * Description : A string reader that wraps StringReader.
 * Created     : 2015/5/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lapis.Script.Parser.Lexical
{
    class Reader : IDisposable
    {
        public Reader(string s)
        {
            _reader = new StringReader(s);
            Line = 1;
            Span = 0;
        }

        public char Peek()
        {
            int i = _reader.Peek();
            if (i > -1)
                return (char)i;
            else
                return '\0';
        }

        public char Read()
        {
            int i = _reader.Read();
            char c;
            if (i > -1)
            {
                c = (char)i;
                if (c == '\n')
                {
                    Line++;
                    Span = 0;
                }
                else
                    Span++;
            }
            else
                c = '\0';
            return c;
        }

        public string Read(int length)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            int i = 0;
            while (i < length && (c = Read()) != '\0')
            {
                sb.Append(c);
                i++;
            }
            return sb.ToString();
        }

        public int Line { get; private set; }

        public int Span { get; private set; }

        public void Dispose()
        {
            _reader.Dispose();
        }


        private StringReader _reader;

    }
}
