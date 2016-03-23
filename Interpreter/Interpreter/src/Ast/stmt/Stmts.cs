using System;

namespace Interpreter
{
    public class Stmts : Statement
    {
        private Statement _left;
        private Stmts _right;

        public Stmts ()
        {
        }

        public Stmts(Statement left, Stmts right) {
            _left = left;
            _right = right;
        }
    }
}

