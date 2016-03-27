using System;

namespace Interpreter
{
    public class ReadStmt : Statement
    {
        public ReadStmt (string name, int row) : base (name, row)
        {
        }
    }
}

