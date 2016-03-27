using System;

namespace Interpreter
{
    public class VarStmt : Statement
    {
        public string Identifier { get; private set; }

        public VarStmt (string identifier, int row) : base (row)
        {
            Identifier = identifier;
        }
    }
}

