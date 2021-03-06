﻿using NUnit.Framework;
using System;
using Interpreter;

namespace InterpreterTests
{
    [TestFixture ()]
    public class ScannerTest
    {
        [Test ()]
        public void TestTokenTypes ()
        {
            string input = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Var);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Identifier);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Colon);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Int);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Assign);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.IntLiteral);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Addition);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.LeftParenthesis);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.IntLiteral);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Multiplication);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.IntLiteral);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.RightParenthesis);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Semicolon);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Print);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Identifier);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Type, Token.Types.Semicolon);
        }

        [Test ()]
        public void TestTokenLexemes ()
        {
            string input = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "var");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "X");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, ":");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "int");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, ":=");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "4");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "+");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "(");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "6");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "*");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "2");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, ")");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, ";");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "print");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, "X");

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Lexeme, ";");
        }

        [Test ()]
        public void TestTokenRows ()
        {
            string input = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 1);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 2);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 2);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Row, 2);
        }

        [Test ()]
        public void TestTokenColumns ()
        {
            string input = "var X : int := 4 + (6 * 2);\n" +
                "print X;";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 0);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 4);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 6);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 8);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 12);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 15);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 17);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 19);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 20);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 22);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 24);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 25);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 26);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 0);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 6);

            token = scanner.GetNextToken ();
            Assert.AreEqual (token.Column, 7);
        }

        [Test ()]
        public void TestEOS() {
            string input = "";
            Scanner scanner = new Scanner (input);
            Token EOS = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, EOS.Type);
            Assert.AreEqual (0, EOS.Column);
            Assert.AreEqual (1, EOS.Row);
        }

        [Test ()]
        public void TestSingleLineCommentsAreRemoved() {
            string input = "//12345 \n";
            Scanner scanner = new Scanner (input);
            Token EOS = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, EOS.Type);
            Assert.AreEqual (9, EOS.Column);
            Assert.AreEqual (1, EOS.Row);
        }

        [Test ()]
        public void TestMultiLineCommentsAreRemoved() {
            string input = "/*12345\n\nasdf*/";
            Scanner scanner = new Scanner (input);
            Token EOS = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, EOS.Type);
            Assert.AreEqual (6, EOS.Column);
            Assert.AreEqual (3, EOS.Row);
        }

        [Test ()]
        public void TestAssignToken() {
            string input = ":=";
            Scanner scanner = new Scanner (input);
            Token assignToken = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Assign, assignToken.Type);
            Assert.AreEqual (0, assignToken.Column);
            Assert.AreEqual (1, assignToken.Row);
            Assert.AreEqual (":=", assignToken.Lexeme);
        }

        [Test ()]
        public void TestEqualToken() {
            string input = "=";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Equal, token.Type);
            Assert.AreEqual (0, token.Column);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual ("=", token.Lexeme);
        }

        [Test ()]
        public void TestColonToken() {
            string input = ":";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Colon, token.Type);
            Assert.AreEqual (0, token.Column);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (":", token.Lexeme);
        }

        [Test ()]
        public void TestRandomTokens() {
            string input = ":() \n:=";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Colon, token.Type);
            Assert.AreEqual (0, token.Column);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (":", token.Lexeme);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.LeftParenthesis, token.Type);
            Assert.AreEqual (1, token.Column);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual ("(", token.Lexeme);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.RightParenthesis, token.Type);
            Assert.AreEqual (2, token.Column);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (")", token.Lexeme);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Assign, token.Type);
            Assert.AreEqual (0, token.Column);
            Assert.AreEqual (2, token.Row);
            Assert.AreEqual (":=", token.Lexeme);
        }

        [Test ()]
        public void TestDigitTokens() {
            string input = "123 0 10\n10";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
            Assert.AreEqual ("123", token.Lexeme);
            Assert.AreEqual (0, token.Column);
            Assert.AreEqual (1, token.Row);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
            Assert.AreEqual ("0", token.Lexeme);
            Assert.AreEqual (4, token.Column);
            Assert.AreEqual (1, token.Row);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
            Assert.AreEqual ("10", token.Lexeme);
            Assert.AreEqual (6, token.Column);
            Assert.AreEqual (1, token.Row);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
            Assert.AreEqual ("10", token.Lexeme);
        }

        [Test ()]
        public void TestDigitAndMinusOperatorTokens() {
            string input = "1-1";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Subtraction, token.Type);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);

            input = "1 - 1";
            scanner = new Scanner (input);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Subtraction, token.Type);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.IntLiteral, token.Type);
        }

        [Test ()]
        public void TestNestedCommentsAreRemoved() {
            string input = "/*12345 \n /* asd asdf if else */ test */=";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Equal, token.Type);
            Assert.AreEqual (31, token.Column);
            Assert.AreEqual (2, token.Row);
            Assert.AreEqual ("=", token.Lexeme);
        }

        [Test ()]
        public void TestTokenBeforeAndAfterMultilineComment() {
            string input = ": /*12*/ =";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Colon, token.Type);
            Assert.AreEqual (0, token.Column);
            Assert.AreEqual (1, token.Row);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Equal, token.Type);
            Assert.AreEqual (9, token.Column);
            Assert.AreEqual (1, token.Row);
        }

        [Test ()]
        public void TestReturnsEOSifMultilineCommentsAreNotClosed() {
            string input = "/*12\n/* asd as/*df if else */ test */\n";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, token.Type);
            Assert.AreEqual (3, token.Row);
            Assert.AreEqual (0, token.Column);

            input = "/*23/*67/*";
            scanner = new Scanner (input);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, token.Type);
            Assert.AreEqual (10, token.Column);
            Assert.AreEqual (1, token.Row);

            input = "/*23/*67/*\n123";
            scanner = new Scanner (input);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, token.Type);
            Assert.AreEqual (3, token.Column);
            Assert.AreEqual (2, token.Row);
        }

        [Test ()]
        public void TestStringLiteralCreation() {
            string input = "\"foo\"=\"bar\"";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.StringLiteral, token.Type);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (0, token.Column);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.Equal, token.Type);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (5, token.Column);

            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.StringLiteral, token.Type);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (6, token.Column);

            input = "\"foo\\bar\"";
            scanner = new Scanner (input);
            token = scanner.GetNextToken ();
            Assert.AreEqual ("foo\\bar", token.Lexeme);
        }

        [Test ()]
        public void TestStringLiteral() {
            Scanner scanner = new Scanner ("\"how many times?\";\n");
            Token token = scanner.GetNextToken ();
            Assert.AreEqual ("how many times?", token.Lexeme);

            scanner = new Scanner ("\"How many times?\";\n");
            token = scanner.GetNextToken ();
            Assert.AreEqual ("How many times?", token.Lexeme);

            scanner = new Scanner ("print \"How many times?\";\n");
            scanner.GetNextToken ();
            token = scanner.GetNextToken ();
            Assert.AreEqual ("How many times?", token.Lexeme);
        }

        [Test ()]
        public void TestStringLiteralCreationWithNoEnd() {
            string input = "\"foo=";
            Scanner scanner = new Scanner (input);
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.ERROR, token.Type);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (0, token.Column);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.EOS, token.Type);
            Assert.AreEqual (1, token.Row);
            Assert.AreEqual (5, token.Column);
        }

        [Test ()]
        public void TestRangeOperator() {
            Scanner scanner = new Scanner ("..");
            Assert.AreEqual (Token.Types.Range, scanner.GetNextToken ().Type);
        }

        [Test ()]
        public void TestBooleans() {
            Scanner scanner = new Scanner ("true false");
            Token token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.BoolLiteral, token.Type);
            Assert.AreEqual ("true", token.Lexeme);
            token = scanner.GetNextToken ();
            Assert.AreEqual (Token.Types.BoolLiteral, token.Type);
            Assert.AreEqual ("false", token.Lexeme);
        }
    }
}