using System;
using System.IO;
using System.Text;
using System.Collections;

namespace Interpreter
{
	public class Scanner
	{
		private int _currentRow = 1;
		private int _currentColumn = 0;
		private int _currentTokenColumn = 0;
		private StreamReader _charStream;

		private static Hashtable operators = new Hashtable {
			{"+", Token.Types.Addition},
			{"-", Token.Types.Subtraction}, 
			{"*", Token.Types.Multiplication},
			{"/", Token.Types.Division},
			{"<", Token.Types.Less},
			{"=", Token.Types.Equal},
			{"&", Token.Types.And},
			{"!", Token.Types.Not},
			{":=", Token.Types.Assign}
		};

		private static Hashtable types = new Hashtable {
			{"int", Token.Types.Int},
			{"string", Token.Types.String},
			{"bool", Token.Types.Bool}
		};

		private static Hashtable reserved_keywords = new Hashtable() {
			{"var", Token.Types.Var}, 
			{"for", Token.Types.For},
			{"end", Token.Types.End},
			{"in", Token.Types.In},
			{"do", Token.Types.Do},
			{"read", Token.Types.Read}, 
			{"print", Token.Types.Print},
			{"int", Token.Types.Int},
			{"string", Token.Types.String},
			{"bool", Token.Types.Bool},
			{"assert", Token.Types.Assert}
		};

		private static Hashtable symbols = new Hashtable() {
			{":", Token.Types.Colon},
			{";", Token.Types.Semicolon},
			{"(", Token.Types.LeftParenthesis},
			{")", Token.Types.RightParenthesis}
		};

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
			int currentChar = getNextChar ();
			string currentChString = "" + (char)currentChar;

			if (isEndOfSource(currentChar)) {
				return new Token (_currentRow, _currentTokenColumn, "", Token.Types.EOS);
			}

			if (operators.ContainsKey(currentChString)) {
				return new Token (_currentRow, _currentTokenColumn, currentChString, (Token.Types)operators[currentChString]);
			}

			if (symbols.ContainsKey(currentChString)) {
				// check for assign operator
				if (currentChar == ':' && (char)peekNextChar() == '=') {
					char nextChar = (char)readNextChar ();
					string lexeme = "" + (char)currentChar + nextChar;
					return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.Assign);
				}
					
				return new Token (_currentRow, _currentTokenColumn, currentChString, (Token.Types)symbols[currentChString]);
			}

			if (Char.IsDigit((char)currentChar)) {
				System.Console.WriteLine("WAS A DIGIT ");
			}

			/**
			 * Beginning of a new Token
			 * - if whitespace or new line (DONE)
			 * 	- skip all of them, increase the column and line numbers (DONE)
			 * - if stream ends, return EOS (end of source) token (DONE)
			 * - if matches / (DONE)
			 * 	- peek (DONE)
			 * 		- if //, then skip the whole line (DONE)
			 * 		- if /* then skip until * / is found (DONE)
			 * - if matches single char token (DONE)
			 * 	- return the token (DONE)
			 * 	- if matched : then must Peek to see if it's := (DONE)
			 * - if matches "
			 * 	- string literal (scan String)
			 * - if digit
			 * - if letter
			 * 	- keyword
			 * 	- identifier 
					**/

					return new Token(1, 2, "asd", Token.Types.Addition);
		}

		public int getNextChar() {
			// System.Console.WriteLine ("Row: " + _currentRow + ", Column: " + _currentColumn);
			_currentTokenColumn = _currentColumn;
			int currentChar = readNextChar ();
			int nextChar = peekNextChar ();

			// System.Console.WriteLine (currentChar + " == " + (char)currentChar);

			// EOS, do not increment the _currentColumn
			if (isEndOfSource(currentChar)) {
				return currentChar;
			}

			// skip whitespaces
			if (isWhitespace(currentChar)) {
				return getNextChar ();
			}

			// skip comments
			if (isComment (currentChar, nextChar)) {
				skipComment ();
				return getNextChar ();
			}
				
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

