using System;

namespace Interpreter
{
    public class AssignmentStmt : Statement
    {
        private Statement _left;
        private Expression _right;

        public AssignmentStmt (Statement varDeclStmt, Expression expr, int row) :base (row)
        {
            _left = varDeclStmt;
            _right = expr;
        }
    }
}

