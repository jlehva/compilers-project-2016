using System;

namespace Interpreter
{
    public class AssertStmt : Statement
    {
        public AssertStmt (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

