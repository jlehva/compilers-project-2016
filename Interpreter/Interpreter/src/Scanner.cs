using System;
using System.IO;
using System.Text;

namespace Interpreter
{
	public class Scanner
	{
		private int _currentRow = 0;
		private int _currentColumn = 0;
		private StreamReader _charStream;
		private TypeFinder _typeFinder;

		// used for tests
		public Scanner(string input) {
			byte[] byteArray = Encoding.UTF8.GetBytes(input);
			MemoryStream stream = new MemoryStream(byteArray);
			StreamReader charStream = new StreamReader(stream);
			_charStream = charStream;
			_typeFinder = new TypeFinder();
		}

		public Scanner(StreamReader charStream) {
			_charStream = charStream;
			_typeFinder = new TypeFinder();
		}

		public Token getNextToken() {
			int current = getNextChar ();

			if (current == -1) {
				return new Token (_currentRow, _currentColumn, "", Token.Types.EOS);
			}

			/**
			 * Beginning of a new Token
			 * - if whitespace or new line
			 * 	- skip all of them, increase the column and line numbers
			 * - if matches /
			 * 	- peek
			 * 		- if //, then skip the whole line
			 * 		- if /* then skip until * / is found
			 * - if matches single char token
			 * 	- return the token
			 * 	- if matched : then must Peek to see if it's :=
			 * - if matches "
			 * 	- string literal (scan String)
			 * - if is number
			 * 	- int literal (scan integer)
			 * - 
					**/

			// if matches single char operator then just return it as a token

			// check if it's a String and return the String token
			if (current == '"') {
				
			}

			// if stream ends, return EOS (end of source) token
			return new Token(1, 2, "asd", _typeFinder.findTypeFor("asd"));
		}

		public int getNextChar() {
			System.Console.WriteLine ("Row: " + _currentRow + ", Column: " + _currentColumn);
			int current = _charStream.Read ();

			System.Console.WriteLine (current + " == " + (char)current);

			// EOS, do not increment the _currentColumn
			if (current == -1) {
				return current;
			}

			if (isWhitespace(current)) {
				return getNextChar ();
			}

			_currentColumn++;

			// increase column number
			return current;
		}

		private String scanString() {
			return "";
		}

		private void skipWhitespaces() {

		}
			
		private bool isWhitespace(int current) {
			// if char is 10 (\n), then increase the line number by 1
			if (current == 10) {
				_currentRow++;
				_currentColumn = 0;
				return true;
			}
				
			// if "normal" whitespace
			if (Char.IsWhiteSpace ((char)current)) {
				_currentColumn++;
				return true;
			}

			return false;
		}
	}
}

