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
            //foreach (Exception e in parser.GetErrors()) {
            //    System.Console.WriteLine (e.Message);
            //}
            Assert.AreEqual (0, parser.GetErrors ().Count);
            // prog.Print ();
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
            Program prog = parser.Parse ();
            Assert.AreEqual (0, parser.GetErrors ().Count);
            // prog.Print ();
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
            Program prog = parser.Parse ();
            Assert.AreEqual (0, parser.GetErrors ().Count);
            prog.Print ();
        }

        [Test ()]
        public void TestVarDeclStmtAst() {
            string app = "var n : int;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            // VarDeclStmt stmt = (VarDeclStmt)prog.Statements.Left;
            // VarDeclStmt expected = new VarDeclStmt (new IntType (1), "n", 1);
            // Assert.AreEqual (stmt.Name, expected.Name);
            // Assert.AreEqual (stmt.Type.ToString (), expected.Type.ToString ());
            // Assert.AreEqual (stmt.Row, expected.Row);
            // VarDeclStmt varDeclStmt = 
            // Assert.AreEqual (prog.Statements.Right.Left, null);
            // Assert.AreEqual (prog.Statements.Right.Right, null);
        }

        [Test ()]
        public void TestAssignmentStmttAst() {
            string app = "var v : int := 1;";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            // VarDeclStmt stmt = (VarDeclStmt)prog.Statements.Left;
            // VarDeclStmt varDeclStmt = new VarDeclStmt (new IntType (1), "v", 1);
            // AssignmentStmt assignmentStmt = new AssignmentStmt (varDeclStmt, );
            // Assert.AreEqual (stmt.Name, expected.Name);
            //Assert.AreEqual (stmt.Type.ToString (), expected.Type.ToString ());
            //Assert.AreEqual (stmt.Row, expected.Row);
        }

        [Test ()]
        public void TestStringExpressionAst() {
            string app = "print \"test\";";
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            // Assert.NotNull (prog.Statements.Left); // Statement
            // Assert.IsNull (prog.Statements.Right); // Statements / tail
            // Assert.AreEqual (prog.Statements.Left.Left);
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
            Parser parser = new Parser (new Scanner (app));
            Program prog = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser ();
            semanticAnalyser.Run (prog);
        }
    }
}

