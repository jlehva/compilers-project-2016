using System;

namespace Interpreter
{
    public class RelationalExpr : Expression
    {
        public RelationalExpr (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

