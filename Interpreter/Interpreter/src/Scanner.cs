using System;
using System.IO;
using System.Text;

namespace Interpreter
{
	public class Scanner
	{
		private int _row = 0;
		private int _column = 0;
		private StreamReader _charStream;

		// used for tests
		public Scanner(string input) {
			byte[] byteArray = Encoding.UTF8.GetBytes(input);
			MemoryStream stream = new MemoryStream(byteArray);
			StreamReader charStream = new StreamReader(stream);
			_charStream = charStream;
		}

		public Scanner(StreamReader charStream) {
			_charStream = charStream;
		}

		public Token getNextToken() {
			var current = getNextChar ();

			// if whitespace == skip char
			if (Char.IsWhiteSpace(current)) {
				// skip
			}

			// line break is char 10

			// if matches single char operator then just return it as a token

			// check if it's a String and return the String token
			if (current == '"') {
				
			}

			// if stream ends, return EOS (end of source) token
			return new Token();
		}

		public char getNextChar() {
			var current = _charStream.Read ();

				System.Console.WriteLine (current + " == " + (char)current);

			if (Char.IsWhiteSpace ((char)current)) {
				System.Console.WriteLine ("char was whitespace");
			}
			// increase column number
				return (char)current;
		}

		private String scanString() {
			return "";
		}
			
		private bool isWhitespace(int current) {
			// if char is 10 (\n), then increase the line number by 1
			if (current == 10) {
				_row++;
				_column = 0;
				return true;
			}
				
			// if "normal" whitespace
			if (Char.IsWhiteSpace ((char)current)) {
				_column++;
				return true;
			}

			return false;
		}
	}
}

