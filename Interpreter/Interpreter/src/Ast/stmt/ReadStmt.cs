using System;

namespace Interpreter
{
    public class ReadStmt : Statement
    {
        public string Identifier { get; private set; }

        public ReadStmt (string identifier, int row) : base (row)
        {
            Identifier = identifier;
        }
    }
}

