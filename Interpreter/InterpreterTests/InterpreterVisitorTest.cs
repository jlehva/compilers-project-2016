using NUnit.Framework;
using System;
using Interpreter;
using System.Collections.Generic;

namespace InterpreterTests
{
    [TestFixture ()]
    public class InterpreterVisitorTest
    {
        [Test ()]
        public void TestInterpretingAppExample1 () {
            string app = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (prog);
            semanticAnalyser.Run ();
            InterpreterVisitor interpreter = new InterpreterVisitor (prog);
            interpreter.Run ();
        }

        [Test ()]
        public void TestInterpretingAppExample2 () {
            string app = 
                "var nTimes : int := 5;\n" +
                "print \"How many times?\";\n" +
                "var x : int;\n" +
                "for x in 0..nTimes-1 do\n" +
                "    print x;\n" +
                "    print \" : Hello, World!\\n\";\n" +
                "end for;\n" +
                "print x;\n" +
                "print nTimes;\n" +
                "assert (x = nTimes);\n";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (prog);
            semanticAnalyser.Run ();
            InterpreterVisitor interpreter = new InterpreterVisitor (prog);

            try {
                interpreter.Run ();
                Assert.Fail();
            } catch (AssertError error) {
                // this is what we want
                Assert.Pass (error.Message);
            } catch (Exception e) {
                Assert.Fail ("Wrong exception: " + e.Message);
            }
        }

        [Test ()]
        public void TestInterpretingAppExample3 () {
            string app = 
                "print \"Give a number\";\n" +
                "var n : int := 4;\n" +
                "var v : int := 1;\n" +
                "var i : int;\n" +
                "for i in 1..n do\n" + 
                "v := v * i;\n" +
                "end for;\n" +
                "print \"The result is: \";\n" +
                "print v;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (prog);
            semanticAnalyser.Run ();
            InterpreterVisitor interpreter = new InterpreterVisitor (prog);
            interpreter.Run ();
        }
    }
}

