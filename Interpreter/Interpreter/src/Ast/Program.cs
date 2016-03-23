using System;

namespace Interpreter
{
    public class Program : Node
    {
        private Stmts _statements;

        public Program (int row) : base (row)
        {
        }

        public Program (Stmts statements, int row) : base (row) {
            _statements = statements;
        }

        public Stmts Statements {
            get { return _statements; }
            set { _statements = value; }
        }
    }
}

