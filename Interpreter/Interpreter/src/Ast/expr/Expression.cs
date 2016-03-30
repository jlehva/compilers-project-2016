using System;

namespace Interpreter
{
    public class Expression : Node
    {
        public Expression (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

