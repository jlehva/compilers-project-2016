using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
	[TestFixture ()]
	public class ScannerTest
	{
		[Test ()]
		public void TestCase ()
		{
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
			
		[Test ()]
		public void FindFirstToken() {
			string input = "var X : int := 4 + (6 * 2);\n" +
				"print X;";
			Scanner scanner = new Scanner (input);
			Token token = scanner.getNextToken ();
			Assert.AreEqual (token.Lexeme, "var");
		}
	}
}

