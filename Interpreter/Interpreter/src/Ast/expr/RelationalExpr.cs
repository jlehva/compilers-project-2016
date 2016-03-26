using System;

namespace Interpreter
{
    public class RelationalExpr : Expression
    {
        public string Lexeme { get; private set; }

        public RelationalExpr (Expression left, Expression right, int row, string lexeme) : base(left, right, row)
        {
            Lexeme = lexeme;
        }
    }
}

