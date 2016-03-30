using System;

namespace Interpreter
{
    public class Program : Node
    {
        public Program (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

