using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
    [TestFixture ()]
    public class ScannerTestAppExampleOne
    {
        [Test ()]
        public void TestAppExampleTwo ()
        {
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
            
            Scanner scanner = new Scanner (app);
            Token token;

            Assert.AreEqual(Token.Types.Var, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Colon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Int, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Assign, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.IntLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Print, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.StringLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Read, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Var, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Colon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Int, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.For, token.Type);
            Assert.AreEqual(5, token.Row);
            Assert.AreEqual(0, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.Identifier, token.Type);
            Assert.AreEqual(4, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.In, token.Type);
            Assert.AreEqual(6, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.IntLiteral, token.Type);
            Assert.AreEqual(9, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.Range, token.Type);
            Assert.AreEqual(10, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.Identifier, token.Type);
            Assert.AreEqual(12, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.Subtraction, token.Type);
            Assert.AreEqual(18, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.IntLiteral, token.Type);
            Assert.AreEqual(19, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual(Token.Types.Do, token.Type);
            Assert.AreEqual(21, token.Column);

            Assert.AreEqual(Token.Types.Print, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Print, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.StringLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.End, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.For, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Assert, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.LeftParenthesis, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Equal, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.RightParenthesis, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.EOS, scanner.GetNextToken ().Type);
        }

        [Test ()]
        public void TestAppExampleThree ()
        {
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

            Scanner scanner = new Scanner (app);

            Assert.AreEqual(Token.Types.Print, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.StringLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Var, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Colon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Int, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Read, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Var, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Colon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Int, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Assign, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.IntLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Var, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Colon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Int, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.For, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.In, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.IntLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Range, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Do, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Assign, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Multiplication, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.End, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.For, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Print, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.StringLiteral, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);

            Assert.AreEqual(Token.Types.Print, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Identifier, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.Semicolon, scanner.GetNextToken ().Type);
            Assert.AreEqual(Token.Types.EOS, scanner.GetNextToken ().Type);
        }
    }
}