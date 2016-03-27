using System;

namespace Interpreter
{
    public class StringValueExpr : Expression
    {
        public StringValueExpr (string name, int row) : base (name, row)
        {
        }
    }
}

