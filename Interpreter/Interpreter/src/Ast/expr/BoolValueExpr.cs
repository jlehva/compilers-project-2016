using System;

namespace Interpreter
{
    public class BoolValueExpr : Expression
    {
        private string _left;

        public BoolValueExpr (string left, int row) : base (row)
        {
            _left = left;
        }
    }
}

