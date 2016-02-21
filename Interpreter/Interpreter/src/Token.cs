using System;

namespace Interpreter
{
	public class Token
	{

		private int _row;
		private int _column;
		private string _lexeme;
		// Token type

		public Token ()
		{
		}

		public Token(int row, int column, string lexeme)
		{
			_row = row;
			_column = column;
			_lexeme = lexeme;
		}
			
		public int Column
		{
			get { return _column; }
			set { _column = value; }
		}

		public int Row
		{
			get { return _row; }
			set { _row = value; }
		}

		public string Lexeme
		{
			get { return _lexeme; }
			set { _lexeme = value; }
		}
	}
}

