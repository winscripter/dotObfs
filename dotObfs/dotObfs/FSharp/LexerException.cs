using System;

namespace dotObfs.Core.FSharp
{
    internal class LexerException : Exception
    {
        public LexerException(string msg)
            : base (msg)
        {
        }

        public LexerException()
        {
        }
    }
}
