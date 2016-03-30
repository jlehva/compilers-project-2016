using System;

namespace Interpreter
{
    public class Statement : Node
    {
        public Statement (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

