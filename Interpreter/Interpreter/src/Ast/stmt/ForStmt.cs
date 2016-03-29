using System;

namespace Interpreter
{
    public class ForStmt : Statement
    {
        public ForStmt (string name, int row) : base(name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

