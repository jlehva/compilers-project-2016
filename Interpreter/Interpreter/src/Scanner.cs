using System;

namespace Interpreter
{
	public class Scanner
	{
		private string input;
		private int row = 0;
		private int column = 0;

		public Scanner(string input) {
			this.input = input;
		}

		public Token getNextToken() {
			return new Token();
		}

		private char getNextChar() {
			return ' ';
		}
	}
}

