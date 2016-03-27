using System;

namespace Interpreter
{
    public class AssignmentStmt : Statement
    {
        private Statement _left; // VarDeclStmt or VarStmt
        private Expression _right;

        public AssignmentStmt (Statement identifier, Expression expr, int row) :base (row)
        {
            _left = identifier;
            _right = expr;
        }
    }
}

