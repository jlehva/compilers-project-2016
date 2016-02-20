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
		}
	}
}

