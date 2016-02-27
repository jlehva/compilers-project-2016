using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
	[TestFixture ()]
	public class ScannerTest
	{
		[Test ()]
		public void TestTokenTypes ()
		{
			string input = "var X : int := 4 + (6 * 2);\n" +
				"print X;";
			Scanner scanner = new Scanner (input);
			Token token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Var);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Identifier);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Colon);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Int);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Assign);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.IntLiteral);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Addition);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.LeftParenthesis);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.IntLiteral);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Multiplication);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.IntLiteral);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.RightParenthesis);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Semicolon);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Print);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Identifier);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Type, Token.Types.Semicolon);
		}

		[Test ()]
		public void TestTokenLexemes ()
		{
			string input = "var X : int := 4 + (6 * 2);\n" +
				"print X;";
			Scanner scanner = new Scanner (input);
			Token token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "var");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "X");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, ":");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "int");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, ":=");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "4");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "+");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "(");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "6");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "*");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "2");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, ")");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, ";");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "print");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "X");

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, ";");
		}

		[Test ()]
		public void TestTokenRows ()
		{
			string input = "var X : int := 4 + (6 * 2);\n" +
				"print X;";
			Scanner scanner = new Scanner (input);
			Token token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 1);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 2);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 2);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Row, 2);
		}

		[Test ()]
		public void TestTokenColumns ()
		{
			string input = "var X : int := 4 + (6 * 2);\n" +
				"print X;";
			Scanner scanner = new Scanner (input);
			Token token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 0);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 4);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 6);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 8);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 12);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 15);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 17);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 19);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 20);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 22);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 24);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 25);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 26);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 0);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 6);

			token = scanner.getNextToken ();
			Assert.AreEqual (token.Column, 7);
		}
			
		[Test ()]
		public void TestIsJustForDebuggingForNow() {
			string input = "var X : int := 4 + (6 * 2);\n" +
				"print X;";
			Scanner scanner = new Scanner (input);
			char first = scanner.getNextChar ();
			Assert.IsNotNull (first);
			Assert.AreEqual (first, 'v');
			for (int i = 1; i <= 30; i++) {
				scanner.getNextChar ();
			}
		}
	}
}

