using System;

namespace Interpreter
{
    public class VarDeclStmt : Statement
    {
        public VarDeclStmt (string name, int row) : base (name, row)
        {
        }
    }
}

