using System;

namespace Interpreter
{
    public class RuntimeError : Error
    {
        public RuntimeError (string message, int row, int column) : base (message, row, column)
        {
        }
    }
}

