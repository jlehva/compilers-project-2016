using System;

namespace Interpreter
{
    public class Expression : Node
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public Expression (int row) : base (row)
        {
        }

        public Expression (Expression left, Expression right, int row) : base(row)
        {
            Left = left;
            Right = right;
        }
    }
}

