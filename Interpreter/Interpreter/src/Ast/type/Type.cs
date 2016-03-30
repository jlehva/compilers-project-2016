using System;

namespace Interpreter
{
    public class Type : Node
    {
        public Type (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

