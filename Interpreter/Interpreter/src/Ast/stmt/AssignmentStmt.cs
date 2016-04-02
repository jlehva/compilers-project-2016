using System;

namespace Interpreter
{
    public class AssignmentStmt : Statement
    {
        public AssignmentStmt (string name, int row, int column) : base (name, row, column)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

