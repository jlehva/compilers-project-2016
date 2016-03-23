using System;

namespace Interpreter
{
    public class UnaryExpr : Expression
    {
        private Expression _unaryOp;
        private Expression _expression;

        public UnaryExpr (Expression unaryOp, Expression expression, int row) : base(row)
        {
            _unaryOp = unaryOp;
            _expression = expression;
        }
    }
}

