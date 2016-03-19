using NUnit.Framework;
using System;
using Interpreter;
using System.Collections.Generic;

namespace InterpreterTests
{
    [TestFixture ()]
    public class ParserTest
    {
        [Test ()]
        public void TestIncorrectStartSymbol () {
            Parser parser = new Parser (new Scanner ("="));
            parser.Parse ();
            List<Exception> errors = parser.GetErrors ();
            Assert.AreEqual (1, errors.Count);
        }

    }
}

