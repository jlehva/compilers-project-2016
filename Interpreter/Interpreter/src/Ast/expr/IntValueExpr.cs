using System;

namespace Interpreter
{
    public class IntValueExpr : Expression
    {
        public IntValueExpr (string name, int row) : base (name, row)
        {
        }
    }
}

