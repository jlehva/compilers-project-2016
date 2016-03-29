using System;

namespace Interpreter
{
    public class ArithmeticExpr : Expression
    {
        public ArithmeticExpr (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

