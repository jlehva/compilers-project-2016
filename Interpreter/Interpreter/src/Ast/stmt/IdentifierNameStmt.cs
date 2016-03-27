using System;

namespace Interpreter
{
    public class IdentifierNameStmt : Statement
    {
        public IdentifierNameStmt (string name, int row) : base (name, row)
        {
        }
    }
}

