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

		private static ArrayList escapeSequences = new ArrayList{"asd"};

		// used for tests
		public Scanner(string input) {
			System.Console.WriteLine (input);
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
				return createEOSToken ();
			}

			if (isIntegerLiteral(currentChar)) {
				return createIntLiteralToken (currentChar);
			}

			if (isOperator(currentChar)) {
				return createOperatorToken (currentChar);
			}

			if (isSymbol(currentChar)) {
				return createSymbolToken (currentChar);
			}

			if (isStringLiteral(currentChar)) {
				return createStringLiteralToken ();
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
				* - if digit (DONE)
			 * - if letter
			 * 	- keyword
			 * 	- identifier
					**/
					return new Token(1, 2, "asd", Token.Types.Addition);
		}

		public int getNextChar() {
			_currentTokenColumn = _currentColumn;
			int currentChar = readNextChar ();
			int nextChar = peekNextChar ();

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

		private Token createStringLiteralToken() {
			// consume the first "-character
			int currentChar = readNextChar();
			string lexeme = "";
			System.Console.WriteLine ("String literal: " + currentChar + " == " + (char)currentChar);

			while(true) {
				if (isEndOfSource (currentChar)) {
					return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.ERROR);
				} 

				if ((char)currentChar == '\\') {
					if (isEndOfSource (peekNextChar ())) {
						return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.ERROR);
					} 
					currentChar = readNextChar ();
					lexeme += "\\" + (char)currentChar;
					continue;
				} else {
					currentChar = readNextChar ();
				}

				if ((char)currentChar == '"') {
					break;
				} else {
					lexeme += (char)currentChar;
				}
			}

			return new Token(_currentRow, _currentTokenColumn, lexeme, Token.Types.StringLiteral);
		}

		private Token createIntLiteralToken(int currentChar) {
			String lexeme = "" + (char)currentChar;
			while (isDigit(peekNextChar())) {
				lexeme += (char)readNextChar ();
			}
			return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.IntLiteral);
		}

		private bool isIntegerLiteral(int currentChar) {
			if (isDigit (currentChar)) {
				return true;
			} else if((char)currentChar == '-' && isDigit(peekNextChar())) {
				return true;
			}

			return false;
		}

		private bool isStringLiteral(int currentChar) {
			return currentChar == 34;
		}

		private bool isOperator(int currentChar) {
			string currentChString = "" + (char)currentChar;
			return operators.ContainsKey (currentChString);
		}

		private bool isSymbol(int currentChar) {
			string currentChString = "" + (char)currentChar;
			return symbols.ContainsKey (currentChString);
		}

		private bool isDigit(int currentChar) {
			return Char.IsDigit ((char)currentChar);
		}

		private Token createOperatorToken(int currentChar) {
			string currentChString = "" + (char)currentChar;
			return new Token (_currentRow, _currentTokenColumn, currentChString, (Token.Types)operators[currentChString]);
		}

		private Token createSymbolToken(int currentChar) {
			// check for assign operator
			if ((char)currentChar == ':' && (char)peekNextChar() == '=') {
				readNextChar (); // consume the "="
				string lexeme = ":=";
				return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.Assign);
			}

			string currentChString = "" + (char)currentChar;
			return new Token (_currentRow, _currentTokenColumn, currentChString, (Token.Types)symbols[currentChString]);
		}

		private Token createEOSToken() {
			return new Token (_currentRow, _currentTokenColumn, "", Token.Types.EOS);
		}

		private bool isComment(int currentChar, int nextChar) {
			if (isMultilineComment(currentChar, nextChar) || (char)nextChar == '/') {
				return true;
			}

			return false;
		}

		private bool isMultilineComment(int currentChar, int nextChar) {
			if ((char)currentChar == '/' && (char)nextChar == '*') {
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
				int multilineNestingLevel = 0;
				while (true) {
					currentChar = readNextChar ();
					// test if there's another /* and count them and then try to find as many */
					// == nested multiline comment == /* asd /* nested comment */ asd */
					if (isMultilineComment(currentChar, peekNextChar())) {
						multilineNestingLevel++;
						// consume the "*" from "/*"
						readNextChar ();
					} else if (currentChar == '*' && (char)peekNextChar() == '/') {
						// consume the '/'
						readNextChar ();
						if (multilineNestingLevel == 0) { // return only if nesting level is 0
							break;
						} else {
							multilineNestingLevel--;
						}
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
				// _currentColumn++;
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
				return true;
			}

			return false;
		}

		private int peekNextChar() {
			return _charStream.Peek ();
		}

		private int readNextChar() {
			int currentChar = _charStream.Read ();
			if (!isEndOfSource(currentChar)) {
				_currentColumn++;
			}
			return currentChar;
		}

		private void incrementRow() {
			_currentRow++;
			_currentColumn = 0;
		}
	}
}

