using System;

namespace Interpreter
{
    public class LogicalExpr : Expression
    {
        public LogicalExpr (Expression left, Expression right, int row) : base(left, right, row)
        {
        }
    }
}

