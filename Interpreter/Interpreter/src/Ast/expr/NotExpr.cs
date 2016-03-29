using System;

namespace Interpreter
{
    public class NotExpr : Expression
    {
        public NotExpr (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

