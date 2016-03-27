using System;

namespace Interpreter
{
    public class IdentifierValueExpr : Expression
    {
        public IdentifierValueExpr (string name, int row) : base (name, row)
        {
        }
    }
}

