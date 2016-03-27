using System;

namespace Interpreter
{
    public class AssignmentStmt : Statement
    {
        public AssignmentStmt (string name, int row) :base (name, row)
        {
        }
    }
}

