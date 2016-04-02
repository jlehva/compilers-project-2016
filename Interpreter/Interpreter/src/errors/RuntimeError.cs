using System;

namespace Interpreter
{
    public class RuntimeError : Exception
    {
        public RuntimeError (string message) : base (message)
        {
        }
    }
}

