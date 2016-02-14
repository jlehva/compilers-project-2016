using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase ()
		{
			Scanner scanner = new Scanner ();
			Assert.NotNull (scanner);
		}
	}
}

