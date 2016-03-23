using System;

namespace Interpreter
{
    public class SemanticError : Exception
    {
        public SemanticError (string message) : base (message)
        {
        }
    }
}

