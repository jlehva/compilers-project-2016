using System;

namespace Interpreter
{
    public class SyntaxError : Exception
    {
        public SyntaxError (string message) : base (message)
        {
        }
    }
}

