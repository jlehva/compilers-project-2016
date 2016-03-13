using System;

namespace Interpreter
{
    public class Token
    {

        private int _row;
        private int _column;
        private string _lexeme;
        private Enum _type;

        public enum Types
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Less,
            Equal,
            And,
            Not,
            Assign,
            Var,
            For,
            End,
            In,
            Do,
            Read,
            Print,
            Int,
            String,
            Bool,
            Assert,
            Identifier,
            Colon,
            Semicolon,
            IntLiteral,
            StringLiteral,
            BoolLiteral,
            LeftParenthesis,
            RightParenthesis,
            NONE,
            EOS,
            ERROR,
            Range
        }

        public Token ()
        {
        }

        public Token (int row, int column, string lexeme, Enum type)
        {
            _row = row;
            _column = column;
            _lexeme = lexeme;
            _type = type;
        }

        public int Column {
            get { return _column; }
            set { _column = value; }
        }

        public int Row {
            get { return _row; }
            set { _row = value; }
        }

        public string Lexeme {
            get { return _lexeme; }
            set { _lexeme = value; }
        }

        public Enum Type {
            get { return _type; }
            set { _type = value; }
        }
    }
}

