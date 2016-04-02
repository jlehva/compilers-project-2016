using System;

namespace Interpreter
{
    public class AssertError : Error
    {
        public AssertError (string message, int row, int column) : base (message, row, column)
        {
        }

        public override string Print() {
            return "Assert Error: " + Message;
        }
    }
}

