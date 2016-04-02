using System;

namespace Interpreter
{
    public class SyntaxError : Error
    {
        public SyntaxError (string message, int row, int column) : base (message, row, column)
        {
        }

        public override string Print() {
            return "Syntax Error: " + Message + " [row: " + Row + ", col: " + Column + "]";
        }
    }
}

