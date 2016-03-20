﻿using NUnit.Framework;
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
            Assert.AreEqual (1, parser.GetErrors ().Count);
        }

        [Test ()]
        public void TestAppExampleOneSyntax () {
            string app = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Parser parser = new Parser (new Scanner (app));
            parser.Parse ();
            Assert.AreEqual (0, parser.GetErrors ().Count);
        }

        [Test ()]
        public void TestAppExampleTwoSyntax () {
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
            Parser parser = new Parser (new Scanner (app));
            parser.Parse ();
            Assert.AreEqual (0, parser.GetErrors ().Count);
        }

        [Test ()]
        public void TestAppExampleThreeSyntax() {
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
            Parser parser = new Parser (new Scanner (app));
            parser.Parse ();
            Assert.AreEqual (0, parser.GetErrors ().Count);
        }
    }
}

