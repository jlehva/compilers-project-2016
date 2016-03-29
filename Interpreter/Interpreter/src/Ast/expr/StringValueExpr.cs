using System;

namespace Interpreter
{
    public class StringValueExpr : Expression
    {
        public StringValueExpr (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

