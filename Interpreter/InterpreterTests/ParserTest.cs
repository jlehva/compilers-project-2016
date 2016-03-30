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

        [Test ()]
        public void TestAppExampleOneSyntax () {
            string app = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
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
                "var i : int;\n" +
                "for i in 1..n do\n" + 
                "v := v * i;\n" +
                "end for;\n" +
                "print \"The result is: \";\n" +
                "print v;";
            Parser parser = new Parser (new Scanner (app));
            parser.Parse ();
            Assert.AreEqual (0, parser.GetErrors ().Count);
        }

        [Test ()]
        public void TestParserFindsTwoLexicalErrors() {
            string app = 
                "print \"Give a number\n" +
                "var n : int;\n" +
                "read n;\n" +
                "var v : int := 1;\n" +
                "var i : int;\n" +
                "for i in 1..n do\n" + 
                "v := v * i;\n" +
                "end for;\n" +
                "print \"The result is: \";\n" +
                "\"foo=";
            Parser parser = new Parser (new Scanner (app));
            parser.Parse ();
            Assert.AreEqual (2, parser.GetErrors ().Count);
            foreach (Exception error in parser.GetErrors ()) {
                Assert.AreEqual (typeof(LexicalError), error.GetType ());
            }
        }

        [Test ()]
        public void TestParserFindsTwoSyntaxErrors() {
            string app = 
                "print \"Give a number\"\n" +
                "var n  int;\n" +
                "read n;\n" +
                "var v : int := 1;\n" +
                "var i : int;\n" +
                "for i in 1..n do\n" +
                "v : v * i;\n" +
                "end for;\n" +
                "print \"The result is: \";\n" +
                "print v;";
            Parser parser = new Parser (new Scanner (app));
            parser.Parse ();
            Assert.AreEqual (2, parser.GetErrors ().Count);
            foreach (Exception error in parser.GetErrors ()) {
                Assert.AreEqual (typeof(SyntaxError), error.GetType ());
            }
        }

        [Test ()]
        public void TestTest () {
            string app = 
                "print \"Give a number\";\n" +
                "var n : int;\n" +
                "read n;\n" +
                "var v : int := 1;\n" +
                "var i : int;\n" +
                "for i in 1..n do\n" + 
                "v := v * i;\n" +
                "end for;\n" +
                "print \"The result is: \";\n" +
                "print v;";
            app = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            // app = "print 1 * 2 + 5 - 7 * 5 + 6 + 9 / 3 * 4 / 5;";
            // app = "print (1 * 2) + 5 - (7 * 5) + 6 + ((9 / 3) * 4) / 5;";
            // app = "print 1 * 2 * 5 * 7 * 5 * 6 * 9 * 3 * 4 * 5;";
            // app = "print 0 + 1 * 2 + 3 * 4 + 5 * 6;";
            // app = "print 1 * 2 & 2 - 5 * 5;";
            // app = "print 1 = 5 < 5;";
            // app = "print 1 + 2;";
            // app = "print 1 + 2;";

            // System.Console.WriteLine (app);
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            // prog.Print ();
            // SemanticAnalyser semanticAnalyser = new SemanticAnalyser ();
            // semanticAnalyser.Run (prog);
        }
    }
}

