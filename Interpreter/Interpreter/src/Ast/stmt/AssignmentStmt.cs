using System;

namespace Interpreter
{
    public class AssignmentStmt : Statement
    {
        public AssignmentStmt (string name, int row) :base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

