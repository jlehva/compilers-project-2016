using System;

namespace Interpreter
{
    public class SemanticError : Error
    {
        public SemanticError (string message, int row, int column) : base (message, row, column)
        {
        }

        public override string Print() {
            return "Semantic Error: " + Message + " [row: " + Row + ", col: " + Column + "]";
        }
    }
}

