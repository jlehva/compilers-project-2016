using System;

namespace Interpreter
{
    public class Stmts : Statement
    {
        private Statement _left;
        private Stmts _right;

        public Stmts (int row) : base(row)
        {
        }

        public Stmts(Statement left, Stmts right, int row) : base(row) {
            _left = left;
            _right = right;
        }

        public Stmts Right {
            get { return _right; }
            set { _right = value; }
        }

        public Statement Left {
            get { return _left; }
            set { _left = value; }
        }
    }
}

