using System;

namespace Interpreter
{
    public class LexicalError : Exception
    {
        public LexicalError (string message) : base (message)
        {
        }
    }
}

