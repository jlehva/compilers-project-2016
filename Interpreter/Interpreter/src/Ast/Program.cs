using System;

namespace Interpreter
{
    public class Program : Node
    {
        private Stmts _statements;

        public Program ()
        {
        }

        public Program (Stmts statements) {
            _statements = statements;
        }

        public Stmts Statements {
            get { return _statements; }
            set { _statements = value; }
        }
    }
}

