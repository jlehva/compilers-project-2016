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
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser ();

            try {
                semanticAnalyser.Run (prog);
                Assert.Fail();
            } catch (SemanticError error) {
                // this is what we want
                Assert.Pass (error.Message);
            } catch (Exception e) {
                Assert.Fail ("Wrong exception: " + e.Message);
            }
        }

        [Test ()]
        public void TestSymbolAlreadyExists () {
            // can't declare other variable named X
            string app = "var X : int := 4 + (2 * 2);\n" +
                "var X : int := 4;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser ();

            try {
                semanticAnalyser.Run (prog);
                Assert.Fail();
            } catch (SemanticError error) {
                // this is what we want
                Assert.Pass (error.Message);
            } catch (Exception e) {
                Assert.Fail ("Wrong exception: " + e.Message);
            }
        }

        [Test ()]
        public void TestAssignWrongTypeOfValue () {
            // X is string, can't assign int value to it
            string app = "var X : string := 4 + (2 * 2);\n" +
                "print X;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser ();

            try {
                semanticAnalyser.Run (prog);
                Assert.Fail();
            } catch (SemanticError error) {
                // this is what we want
                Assert.Pass (error.Message);
            } catch (Exception e) {
                Assert.Fail ("Wrong exception: " + e.Message);
            }
        }

        [Test ()]
        public void TestRelationalExpressionOnlyForIntValues () {
            // if relational expression, then both of the operands must be integers
            string app = "print \"error\" < 5;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser ();

            try {
                semanticAnalyser.Run (prog);
                Assert.Fail();
            } catch (SemanticError error) {
                // this is what we want
                Assert.Pass (error.Message);
            } catch (Exception e) {
                Assert.Fail ("Wrong exception: " + e.Message);
            }
        }
    }
}

