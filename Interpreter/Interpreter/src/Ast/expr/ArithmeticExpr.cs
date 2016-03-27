using System;

namespace Interpreter
{
    public class ArtihmeticExpr : Expression
    {
        public string Lexeme { get; private set; }

        public ArtihmeticExpr (Expression left, Expression right, int row, string lexeme) : base(left, right, row)
        {
            Lexeme = lexeme;
        }
    }
}

