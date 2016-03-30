using System;

namespace Interpreter
{
    public class Stmts : Statement
    {
        public Stmts (string name, int row) : base(name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

