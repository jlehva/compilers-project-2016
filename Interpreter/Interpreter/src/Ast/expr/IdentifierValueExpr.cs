using System;

namespace Interpreter
{
    public class IdentifierValueExpr : Expression
    {
        private string _left;

        public IdentifierValueExpr (string left, int row) : base (row)
        {
            _left = left;
        }
    }
}

