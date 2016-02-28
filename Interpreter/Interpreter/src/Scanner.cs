using System;
using System.IO;
using System.Text;

namespace Interpreter
{
	public class Scanner
	{
		private int _currentRow = 1;
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
			int currentChar = getNextChar ();

			if (currentChar == -1) {
				return new Token (_currentRow, _currentColumn, "", Token.Types.EOS);
			}
				

			/**
			 * Beginning of a new Token
			 * - if whitespace or new line (DONE)
			 * 	- skip all of them, increase the column and line numbers (DONE)
			 * - if stream ends, return EOS (end of source) token (DONE)
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

			return new Token(1, 2, "asd", _typeFinder.findTypeFor("asd"));
		}

		public int getNextChar() {
			System.Console.WriteLine ("Row: " + _currentRow + ", Column: " + _currentColumn);
			int currentChar = _charStream.Read ();
			int nextChar = peekNextChar ();

			System.Console.WriteLine (currentChar + " == " + (char)currentChar);

			// EOS, do not increment the _currentColumn
			if (isEndOfSource(currentChar)) {
				return currentChar;
			}

			// skip whitespaces
			if (isWhitespace(currentChar)) {
				return getNextChar ();
			}

			_currentColumn++;

			if (isComment (currentChar, nextChar)) {
				System.Console.WriteLine ("COMMENT FOUND!");
				skipComment ();
				return getNextChar ();
			}

			// increase column number
			return currentChar;
		}

		private String scanString() {
			return "";
		}

		private bool isComment(int currentChar, int nextChar) {
			if ((char)currentChar == '/' && ((char)nextChar == '*' || (char)nextChar == '/')) {
				return true;
			}

			return false;
		}

		private void skipComment() {
			// consume the next / or * from the comment starting symbol
			var currentChar = readNextChar();

			// consume characters until end of line is found
			if ((char)currentChar == '/') {
				while (true) {
					currentChar = readNextChar ();

					if (isEndOfLine(currentChar) || isEndOfSource(peekNextChar())) {
						break;
					}
				}
			} else {
				while (true) {
					currentChar = readNextChar ();

					if (currentChar == '*' && (char)peekNextChar() == '/') {
						// consume the '/' and return
						readNextChar ();
						break;
					} else if (isEndOfSource(currentChar)) {
						break;
					} else if (isEndOfLine(currentChar)) {
						incrementRow ();
					}
				}
			}
		}
			
		private bool isWhitespace(int currentChar) {
			// if char is 10 (\n), then increase the line number by 1
			if (isEndOfLine(currentChar)) {
				incrementRow ();
				return true;
			}
				
			// if "normal" whitespace
			if (Char.IsWhiteSpace ((char)currentChar)) {
				_currentColumn++;
				return true;
			}

			return false;
		}

		private bool isEndOfLine(int currentChar) {
			if (currentChar == 10) {
				return true;
			}

			return false;
		}

		private bool isEndOfSource(int currentChar) {
			if (currentChar == -1) {
				System.Console.WriteLine ("END OF SOURCE");
				return true;
			}

			return false;
		}

		private int peekNextChar() {
			return _charStream.Peek ();
		}

		private int readNextChar() {
			int currentChar = _charStream.Read ();
			_currentColumn++;
			return currentChar;
		}

		private void incrementRow() {
			_currentRow++;
			_currentColumn = 0;
		}
	}
}

