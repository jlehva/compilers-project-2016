using System;

namespace Interpreter
{
    public class ReadStmt : Statement
    {
        public ReadStmt (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

