using System;

namespace Interpreter
{
    public class VarStmt : Statement
    {
        public VarStmt (string name, int row) : base (name, row)
        {
        }
    }
}

