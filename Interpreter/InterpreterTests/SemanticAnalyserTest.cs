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
        public void TestArithmeticOperationOnBoolAndInt () {
            // can't do Bool * Int
            string app = "var X : int := 4 + (true * 2);\n" +
                "print X;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestSymbolAlreadyExists () {
            // can't declare other variable named X
            string app = "var X : int := 4 + (2 * 2);\n" +
                "var X : int := 4;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestAssignWrongTypeOfValue () {
            // X is string, can't assign int value to it
            string app = "var X : string := 4 + (2 * 2);\n" +
                "print X;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestAssignWrongTypeOfValueForAlreadyDeclaredVar () {
            // X is string, can't assign int value to it
            string app = "var X : int := 4;\n" +
                "X := \"ERROR\";";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestRelationalExpressionOnlyForIntValues () {
            // if relational expression, then both of the operands must be integers
            string app = "print \"error\" < 5;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
                semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestTypestackEmptyAfterPrint () {
            string app = 
                "print \"Give a number\";";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestTypestackEmptyAfterExpression () {
            string app = 
                "var X : int := 1 * 2 + 3 * 4 + 5 * 6;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestTypestackEmptyAfterRead () {
            string app = 
                "var n : int;\n" +
                "read n;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestTypestackEmptyAfterDeclarations () {
            string app = 
                "var v : int := 1;\n" +
                "var i : int;\n";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
        }

        [Test ()]
        public void TestTypestackEmptyAfterPrints () {
            string app = 
                "var v : int := 1;\n" +
                "print \"Give a number\";\n" +
                "print \"The result is: \";\n" +
                "print v;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
        }

        [Test ()]
        public void TestTypestackEmptyAfterFor () {
            string app = 
                "var n : int := 3;\n" +
                "var v : int := 1;\n" +
                "var i : int;\n" +
                "for i in 1..n do\n" +
                "v := v * i;\n" +
                "end for;\n";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
        }

        [Test ()]
        public void TestRangeContainsString () {
            // if relational expression, then both of the operands must be integers
            string app = 
                "var n : string := \"SEMANTIC ERROR\";\n" +
                "var v : int := 1;\n" +
                "var i : int;\n" +
                "for i in 1..n do\n" +
                "v := v * i;\n" +
                "end for;\n";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());

                semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestReadVariableMustBeDeclared () {
            string app = 
                "read n;";
            Parser parser = new Parser (new Scanner (app));
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestTypestackEmptyForExampleApp () {
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
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (parser.Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
        }

        [Test ()]
        public void TestAndExpressions () {
            Program prog;

            string app = "var i : bool := true & false;";
            prog = new Parser (new Scanner (app)).Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (prog);
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);

            app = "var i : bool := true & 2;";
            semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());

            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.TypeStack.Count);
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestEqualsExpressions () {
            Program prog;

            string app = "var i : bool := true = false;";
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);

            app = "var i : bool := \"a\" = \"b\";";
            semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);

            app = "var i : bool := 1 = 1;";
            semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);

            app = "var i : bool := \"this fails\" = true;";
            prog = new Parser (new Scanner (app)).Parse ();
            semanticAnalyser = new SemanticAnalyser (prog);
            semanticAnalyser.Run ();

            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestNotExpressions () {
            string app = "var i : bool := !true;";
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);

            app = "var i : bool := !(2 = 2);";
            semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (0, semanticAnalyser.Errors.Count);

            app = "var i : bool := !2;";
            semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
            Assert.AreEqual (1, semanticAnalyser.Errors.Count);
        }

        [Test ()]
        public void TestNoKeyNotFoundException () {
            string app = "print \"test\";" +
                         "var x : int = \"lol\";" +
                         "var y : int = x;" +
                         "print y;";
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (new Parser (new Scanner (app)).Parse ());
            semanticAnalyser.Run ();
        }
    }
}

