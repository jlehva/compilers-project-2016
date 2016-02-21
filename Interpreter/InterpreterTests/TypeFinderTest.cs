using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
	[TestFixture ()]
	public class TypeFinderTest
	{
		[Test ()]
		public void MatchTypeTest() {
			TypeFinder typeFinder = new TypeFinder ();
			typeFinder.findTypeFor ("var");
		}


	}
}

