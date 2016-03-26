using System;

namespace Interpreter
{
    public class NotExpr : Expression
    {
        public NotExpr (Expression left, Expression right, int row) : base(left, right, row)
        {
        }
    }
}

