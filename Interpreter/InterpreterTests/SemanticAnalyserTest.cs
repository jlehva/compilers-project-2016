using NUnit.Framework;
using System;
using Interpreter;
using System.Collections.Generic;

namespace InterpreterTests
{
    [TestFixture ()]
    public class SemanticAnalyserTest
    {
        [Test ()]
        public void TestIncorrectStartSymbol () {
            Parser parser = new Parser (new Scanner ("="));
            try {
                parser.Parse ();
                Assert.Fail();
            } catch (SyntaxError error) {
                // this is what we want
                Assert.Pass (error.Message);
            } catch (Exception e) {
                Assert.Fail ("Wrong exception " + e.Message);
            }
        }
    }
}

