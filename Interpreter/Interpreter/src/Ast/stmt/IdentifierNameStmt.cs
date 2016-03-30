using System;

namespace Interpreter
{
    public class IdentifierNameStmt : Statement
    {
        public IdentifierNameStmt (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

