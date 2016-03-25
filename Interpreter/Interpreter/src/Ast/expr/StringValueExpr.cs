using System;

namespace Interpreter
{
    public class StringValueExpr : Expression
    {
        public string Left { get; private set; }

        public StringValueExpr (string left, int row) : base (row)
        {
            Left = left;
        }
    }
}

