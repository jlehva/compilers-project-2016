using System;

namespace Interpreter
{
    public class IntValueExpr : Expression
    {
        public IntValueExpr (string name, int row, int column) : base (name, row, column)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

