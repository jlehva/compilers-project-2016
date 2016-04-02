using System;

namespace Interpreter
{
    public class ReadStmt : Statement
    {
        public ReadStmt (string name, int row, int column) : base (name, row, column)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

