using System;

namespace Interpreter
{
    public class AssertStmt : Statement
    {
        public Expression AssertExpression { get; private set; }

        public AssertStmt (Expression assertExpr, int row) : base (row)
        {
            AssertExpression = assertExpr;
        }
    }
}

