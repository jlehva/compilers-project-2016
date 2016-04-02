using System;

namespace Interpreter
{
    public class AssertError : Exception
    {
        public AssertError (string message) : base (message)
        {
        }
    }
}

