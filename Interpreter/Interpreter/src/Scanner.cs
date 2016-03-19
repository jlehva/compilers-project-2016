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
            { "+", Token.Types.Addition },
            { "-", Token.Types.Subtraction }, 
            { "*", Token.Types.Multiplication },
            { "/", Token.Types.Division },
            { "<", Token.Types.Less },
            { "=", Token.Types.Equal },
            { "&", Token.Types.And },
            { "!", Token.Types.Not },
            { ":=", Token.Types.Assign },
            { "..", Token.Types.Range }
        };

        private static Hashtable reserved_keywords = new Hashtable () {
            { "var", Token.Types.Var }, 
            { "for", Token.Types.For },
            { "end", Token.Types.End },
            { "in", Token.Types.In },
            { "do", Token.Types.Do },
            { "read", Token.Types.Read }, 
            { "print", Token.Types.Print },
            { "int", Token.Types.Int },
            { "string", Token.Types.String },
            { "bool", Token.Types.Bool },
            { "assert", Token.Types.Assert },
            { "true", Token.Types.BoolLiteral },
            { "false", Token.Types.BoolLiteral }
        };

        private static Hashtable symbols = new Hashtable () {
            { ":", Token.Types.Colon },
            { ";", Token.Types.Semicolon },
            { "(", Token.Types.LeftParenthesis },
            { ")", Token.Types.RightParenthesis }
        };

        // used for tests
        public Scanner (string input)
        {
            System.Console.WriteLine (input);
            byte[] byteArray = Encoding.UTF8.GetBytes (input);
            MemoryStream stream = new MemoryStream (byteArray);
            StreamReader charStream = new StreamReader (stream);
            _charStream = charStream;
        }

        public Scanner (StreamReader charStream)
        {
            _charStream = charStream;
        }

        public Token GetNextToken ()
        {
            int currentChar = getNextChar ();

            if (isEndOfSource (currentChar)) {
                return createEOSToken ();
            }

            if (isIntegerLiteral (currentChar)) {
                return createIntLiteralToken (currentChar);
            }

            if (isOperator (currentChar)) {
                return createOperatorToken (currentChar);
            }

            if (isSymbol (currentChar)) {
                return createSymbolToken (currentChar);
            }

            if (isStringLiteral (currentChar)) {
                return createStringLiteralToken ();
            }

            if (isLetter (currentChar)) {
                return createKeywordOrIdentifierToken (currentChar);
            }

            return new Token (_currentRow, _currentTokenColumn, "" + (char)currentChar, Token.Types.ERROR);
        }

        private int getNextChar ()
        {
            _currentTokenColumn = _currentColumn;
            int currentChar = readNextChar ();
            int nextChar = peekNextChar ();

            // EOS, do not increment the _currentColumn
            if (isEndOfSource (currentChar)) {
                return currentChar;
            }

            // skip whitespaces and end of line
            if (skipWhitespaceAndEndOfLine (currentChar)) {
                return getNextChar ();
            }

            // skip comments
            if (isComment (currentChar, nextChar)) {
                skipComment ();
                return getNextChar ();
            }
                
            return currentChar;
        }

        private Token createStringLiteralToken ()
        {
            // consume the first "-character
            int currentChar = readNextChar ();
            string lexeme = "";

            while (true) {
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

            return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.StringLiteral);
        }

        private Token createKeywordOrIdentifierToken (int currentChar)
        {
            bool malformedToken = false;
            string lexeme = "" + (char)currentChar;

            while (!isEndOfKeywordOrIdentifier (peekNextChar ())) {
                currentChar = readNextChar ();

                // expect the lexeme to only contain letters
                if (!isLetter (currentChar)) {
                    malformedToken = true;
                }

                lexeme += (char)currentChar;
            }

            if (malformedToken) {
                return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.ERROR);
            }

            if (reserved_keywords.ContainsKey (lexeme)) {
                return new Token (_currentRow, _currentTokenColumn, lexeme, (Token.Types)reserved_keywords [lexeme]);
            }

            return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.Identifier);
        }

        private Token createIntLiteralToken (int currentChar)
        {
            String lexeme = "" + (char)currentChar;
            while (isDigit (peekNextChar ())) {
                lexeme += (char)readNextChar ();
            }
            return new Token (_currentRow, _currentTokenColumn, lexeme, Token.Types.IntLiteral);
        }

        private Token createOperatorToken (int currentChar)
        {
            // check for assign operator
            if ((char)currentChar == ':' && (char)peekNextChar () == '=') {
                readNextChar (); // consume the "="
                return new Token (_currentRow, _currentTokenColumn, ":=", Token.Types.Assign);
            } else if ((char)currentChar == '.' && (char)peekNextChar () == '.') {
                readNextChar (); // consume the next "."
                return new Token (_currentRow, _currentTokenColumn, "..", Token.Types.Range);
            }
            string lexeme = "" + (char)currentChar;
            return new Token (_currentRow, _currentTokenColumn, lexeme, (Token.Types)operators [lexeme]);
        }

        private Token createSymbolToken (int currentChar)
        {
            string lexeme = "" + (char)currentChar;
            return new Token (_currentRow, _currentTokenColumn, lexeme, (Token.Types)symbols [lexeme]);
        }

        private Token createEOSToken ()
        {
            return new Token (_currentRow, _currentTokenColumn, "", Token.Types.EOS);
        }

        private void skipComment ()
        {
            // consume the next / or * from the comment starting symbol
            var currentChar = readNextChar ();
            // consume characters until end of line is found
            if ((char)currentChar == '/') {
                while (true) {
                    currentChar = readNextChar ();
                    if (isEndOfLine (currentChar) || isEndOfSource (peekNextChar ())) {
                        break;
                    }
                }
            } else {
                int multilineNestingLevel = 0;
                while (true) {
                    currentChar = readNextChar ();
                    // test if there's another /* and count them and then try to find as many */
                    // == nested multiline comment == /* asd /* nested comment */ asd */
                    if (isMultilineComment (currentChar, peekNextChar ())) {
                        multilineNestingLevel++;
                        // consume the "*" from "/*"
                        readNextChar ();
                    } else if (currentChar == '*' && (char)peekNextChar () == '/') {
                        // consume the '/'
                        readNextChar ();
                        if (multilineNestingLevel == 0) { // return only if nesting level is 0
                            break;
                        } else {
                            multilineNestingLevel--;
                        }
                    } else if (isEndOfSource (currentChar)) {
                        break;
                    } else if (isEndOfLine (currentChar)) {
                        incrementRowAndResetColumn ();
                    }
                }
            }
        }

        private bool skipWhitespaceAndEndOfLine (int currentChar)
        {
            // if char is 10 (\n), then increase the line number by 1
            if (isEndOfLine (currentChar)) {
                incrementRowAndResetColumn ();
                return true;
            }

            return isWhitespace (currentChar);
        }

        private bool isIntegerLiteral (int currentChar)
        {
            if (isDigit (currentChar)) {
                return true;
            }

            return false;
        }

        private bool isStringLiteral (int currentChar)
        {
            return currentChar == 34;
        }

        private bool isOperator (int currentChar)
        {
            string lexeme = "" + (char)currentChar;
            if ((char)currentChar == ':' || (char)currentChar == '.') {
                lexeme += (char)peekNextChar ();
            }

            return operators.ContainsKey (lexeme);
        }

        private bool isSymbol (int currentChar)
        {
            string lexeme = "" + (char)currentChar;
            return symbols.ContainsKey (lexeme);
        }

        private bool isLetter (int currentChar)
        {
            return Char.IsLetter ((char)currentChar);
        }

        private bool isDigit (int currentChar)
        {
            return Char.IsDigit ((char)currentChar);
        }

        private bool isComment (int currentChar, int nextChar)
        {
            if (isMultilineComment (currentChar, nextChar) || (char)nextChar == '/') {
                return true;
            }

            return false;
        }

        private bool isMultilineComment (int currentChar, int nextChar)
        {
            if ((char)currentChar == '/' && (char)nextChar == '*') {
                return true;
            }

            return false;
        }

        private bool isWhitespace (int currentChar)
        {
            // if "normal" whitespace
            if (Char.IsWhiteSpace ((char)currentChar)) {
                return true;
            }

            return false;
        }

        private bool isEndOfLine (int currentChar)
        {
            if (currentChar == 10) {
                return true;
            }

            return false;
        }

        private bool isEndOfSource (int currentChar)
        {
            if (currentChar == -1) {
                return true;
            }

            return false;
        }

        private bool isEndOfKeywordOrIdentifier (int currentChar)
        {
            if (isWhitespace (currentChar)) {
                return true;
            } else if (isEndOfLine (currentChar)) {
                return true;
            } else if (isEndOfSource (currentChar)) {
                return true;
            } else if (isSymbol (currentChar)) {
                return true;
            } else if (isOperator (currentChar)) {
                return true;
            } else {
                return false;
            }
        }

        private int peekNextChar ()
        {
            return _charStream.Peek ();
        }

        private int readNextChar ()
        {
            int currentChar = _charStream.Read ();
            if (!isEndOfSource (currentChar)) {
                _currentColumn++;
            }
            return currentChar;
        }

        private void incrementRowAndResetColumn ()
        {
            _currentRow++;
            _currentColumn = 0;
        }
    }
}