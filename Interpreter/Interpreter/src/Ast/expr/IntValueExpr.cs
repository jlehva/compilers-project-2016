using System;

namespace Interpreter
{
    public class IntValueExpr : Expression
    {
        private string _left;

        public IntValueExpr (string left, int row) : base (row)
        {
            _left = left;
        }
    }
}

