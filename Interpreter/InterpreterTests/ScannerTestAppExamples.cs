using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
	[TestFixture ()]
	public class ScannerTestAppExampleOne
	{
		[Test ()]
		public void TestAppExampleOne ()
		{
			string app = 
				"var nTimes : int := 0;\n" +
				"print \"How many times?\";\n" +
				"read nTimes;\n" +
				"var x : int;\n" +
				"for x in 0..nTimes-1 do\n" +
				"    print x;\n" +
				"    print \" : Hello, World!\\n\";\n" +
				"end for;\n" +
				"assert (x = nTimes);\n";
			
			Scanner scanner = new Scanner (app);

			Assert.AreEqual(Token.Types.Var, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Colon, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Int, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Assign, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.IntLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Print, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.StringLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Read, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Var, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Colon, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Int, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.For, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.In, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.IntLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Range, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Subtraction, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.IntLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Do, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Print, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Print, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.StringLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.End, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.For, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Assert, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.LeftParenthesis, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Equal, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.RightParenthesis, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);
		}

		[Test ()]
		public void TestAppExampleTwo ()
		{
			string app = 
				"print \"Give a number\";\n" +
				"var n : int;\n" +
				"read n;\n" +
				"var v : int := 1;\n" +
				"var i : int;" +
				"for i in 1..n do\n" + 
				"v := v * i;\n" +
				"end for;\n" +
				"print \"The result is: \";\n" +
				"print v;";

			Scanner scanner = new Scanner (app);

			Assert.AreEqual(Token.Types.Print, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.StringLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Var, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Colon, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Int, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Read, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Var, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Colon, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Int, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Assign, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.IntLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Var, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Colon, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Int, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.For, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.In, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.IntLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Range, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Do, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Assign, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Multiplication, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.End, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.For, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Print, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.StringLiteral, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);

			Assert.AreEqual(Token.Types.Print, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Identifier, scanner.getNextToken ().Type);
			Assert.AreEqual(Token.Types.Semicolon, scanner.getNextToken ().Type);
		}
	}
}

