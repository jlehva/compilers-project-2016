﻿using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class Parser
    {
        private Scanner scanner;
        private Token currentToken;
        public List<Error> Errors { get; private set;}

        public Parser (Scanner scanner)
        {
            this.scanner = scanner;
            Errors = new List<Error> (); 
        }

        public Program Parse ()
        {
            ReadNextToken ();
            Program program = Prog ();
            return program;
        }

        private Program Prog ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Var ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.For ||
                (Token.Types)currentToken.Type == Token.Types.Read ||
                (Token.Types)currentToken.Type == Token.Types.Print ||
                (Token.Types)currentToken.Type == Token.Types.Assert) {
                Program program = new Program ("program", currentToken.Row, currentToken.Column);
                program.AddChild (Stmts ());
                return program;
            }

            throw new SyntaxError ("invalid start symbol of a program " + currentToken.Type, 
                currentToken.Row, currentToken.Column);
        }

        private Stmts Stmts ()
        {
            Stmts statements = new Stmts ("stmts", currentToken.Row, currentToken.Column);

            if ((Token.Types)currentToken.Type == Token.Types.Var ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.For ||
                (Token.Types)currentToken.Type == Token.Types.Read ||
                (Token.Types)currentToken.Type == Token.Types.Print ||
                (Token.Types)currentToken.Type == Token.Types.Assert) {
                try {
                    statements.AddChild (Stmt());
                    Match (Token.Types.Semicolon);
                    statements.AddChild (Stmts ());
                    return statements;
                } catch (Error e) {
                    Errors.Add (e);
                    SkipToNextStatement ();
                    return Stmts ();
                }
            } else if ((Token.Types)currentToken.Type == Token.Types.End ||
                       (Token.Types)currentToken.Type == Token.Types.EOS) {
                return statements;
            } 

            throw new SyntaxError ("invalid start symbol for statement " + currentToken.Lexeme, 
                currentToken.Row, currentToken.Column);
        }

        private Statement Stmt ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Var:
                    VarDeclStmt varDeclStmt = new VarDeclStmt ("VarDecl", currentToken.Row, currentToken.Column);
                    Match (Token.Types.Var);
                    varDeclStmt.AddChild (IdentifierNameStmt ());
                    Match (Token.Types.Colon);
                    varDeclStmt.AddChild (Type ());

                    if ((Token.Types)currentToken.Type == Token.Types.Assign) {
                        Match (Token.Types.Assign);
                        varDeclStmt.AddChild (Expr ());
                        return varDeclStmt;
                    } else if ((Token.Types)currentToken.Type == Token.Types.Semicolon) {
                        return varDeclStmt;
                    }

                    throw new SyntaxError ("Expected Assign, got: " + currentToken.Type, currentToken.Row, currentToken.Column);
                case Token.Types.Identifier:
                    AssignmentStmt assignmentStmt = new AssignmentStmt ("AssignmentStmt", currentToken.Row, currentToken.Column);
                    assignmentStmt.AddChild (IdentifierNameStmt ());
                    Match (Token.Types.Assign);
                    assignmentStmt.AddChild (Expr ());
                    return assignmentStmt;
                case Token.Types.For:
                    ForStmt forStmt = new ForStmt ("ForStmt", currentToken.Row, currentToken.Column);
                    Match (Token.Types.For);
                    forStmt.AddChild (IdentifierNameStmt ());
                    Match (Token.Types.In);
                    forStmt.AddChild (Expr ());
                    Match (Token.Types.Range);
                    forStmt.AddChild (Expr ());
                    Match (Token.Types.Do);
                    forStmt.AddChild (Stmts ());
                    Match (Token.Types.End);
                    Match (Token.Types.For);
                    return forStmt;
                case Token.Types.Read:
                    ReadStmt readStmt = new ReadStmt ("ReadStmt", currentToken.Row, currentToken.Column);
                    Match (Token.Types.Read);
                    readStmt.AddChild (IdentifierNameStmt ());
                    return readStmt;
                case Token.Types.Print:
                    PrintStmt printStmt = new PrintStmt ("PrintStmt", currentToken.Row, currentToken.Column);
                    Match (Token.Types.Print);
                    printStmt.AddChild (Expr ());
                    return printStmt;
                case Token.Types.Assert:
                    AssertStmt assertStmt = new AssertStmt ("AssertStmt", currentToken.Row, currentToken.Column);
                    Match (Token.Types.Assert);
                    Match (Token.Types.LeftParenthesis);
                    assertStmt.AddChild (Expr ());
                    Match (Token.Types.RightParenthesis);
                    return assertStmt;
                default:
                    throw new SyntaxError ("invalid start symbol for statement " + currentToken.Lexeme,
                        currentToken.Row, currentToken.Column);
            }
        }

        private IdentifierNameStmt IdentifierNameStmt () 
        {
            Token token = Match (Token.Types.Identifier);
            IdentifierNameStmt identifierNameStmt = new IdentifierNameStmt(token.Lexeme, token.Row, token.Column);
            return identifierNameStmt;
        }
            
        private Expression Expr ()
        {
            Expression expr = new Expression ("expr", currentToken.Row, currentToken.Column);
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {

                Expression left = Conjuct ();
                Expression right = ExprLogical ();

                if (right.Name != null) {
                    right.AddChild (left);
                    expr.AddChild (right);
                    return expr;
                } else {
                    expr.AddChild (left);
                    return expr;
                }
            } else if ((Token.Types)currentToken.Type == Token.Types.Not) {
                NotExpr notExpr = new NotExpr ("!", currentToken.Row, currentToken.Column);
                Match (Token.Types.Not);
                notExpr.AddChild (Expr ());
                return notExpr;
            }

            throw new SyntaxError ("invalid token to start expression " + currentToken.Type,
            currentToken.Row, currentToken.Column);
            
        }

        private Expression ExprLogical ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.And) {
                LogicalExpr logicalExpr = new LogicalExpr ("&", currentToken.Row, currentToken.Column);
                Match (Token.Types.And);

                logicalExpr.AddChild (Conjuct ());
                Expression expression = ExprLogical ();

                if (expression.Name != null) {
                    logicalExpr.AddChild (expression);
                }

                return logicalExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return new LogicalExpr(null, currentToken.Row, currentToken.Column);
            }
                
            throw new SyntaxError ("invalid token to start expression " + currentToken.Type,
            currentToken.Row, currentToken.Column);
        }

        private Expression Conjuct ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {

                Expression left = ExprC ();
                Expression right = ExprRelational ();

                if (right.Name != null) {
                    right.AddChild (left);
                    return right;
                } else {
                    return left;
                }
            }
                
            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row,
            currentToken.Column);
        }

        private Expression ExprRelational ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.Less) {

                RelationalExpr relationalExpr;

                if ((Token.Types)currentToken.Type == Token.Types.Equal) {
                    Match (Token.Types.Equal);
                    relationalExpr = new RelationalExpr ("=", currentToken.Row, currentToken.Column);
                } else {
                    Match (Token.Types.Less);
                    relationalExpr = new RelationalExpr ("<", currentToken.Row, currentToken.Column);
                }

                relationalExpr.AddChild (ExprC ());
                Expression expression = ExprRelational ();

                if (expression.Name != null) {
                    relationalExpr.AddChild (expression);
                }

                return relationalExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return new RelationalExpr(null, currentToken.Row, currentToken.Column);
            }
             
            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row, 
                currentToken.Column);
        }

        private Expression ExprC ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {

                Expression left = Term ();
                Expression right = ExprAdd ();

                if (right.Name != null) {
                    right.AddChild (left); // this adds the third child to right if there's 1 + 2 + 3 (same operator many times in a row)
                    return right;
                } else {
                    return left;
                }
            }

            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row, 
                currentToken.Column);
        }

        private Expression ExprAdd ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                (Token.Types)currentToken.Type == Token.Types.Subtraction) {

                ArithmeticExpr arithmeticExpr;

                if ((Token.Types)currentToken.Type == Token.Types.Addition) {   
                    Match (Token.Types.Addition);
                    arithmeticExpr = new ArithmeticExpr ("+", currentToken.Row, currentToken.Column);
                } else {
                    Match (Token.Types.Subtraction);
                    arithmeticExpr = new ArithmeticExpr ("-", currentToken.Row, currentToken.Column);
                }

                arithmeticExpr.AddChild (Term ());
                Expression expression = ExprAdd ();

                if (expression.Name != null) {
                    arithmeticExpr.AddChild (expression);
                }

                return arithmeticExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return new ArithmeticExpr(null, currentToken.Row, currentToken.Column);
            }

            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row, 
                currentToken.Column);
        }

        private Expression Term ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {

                Expression left = Factor ();
                Expression right = ExprMult ();

                if (right.Name != null) {
                    right.AddChild (left);
                    return right;
                } else {
                    return left;
                }
            }
                
            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row, 
                currentToken.Column);
        }

        private Expression ExprMult ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Multiplication ||
                (Token.Types)currentToken.Type == Token.Types.Division) {

                ArithmeticExpr arithmeticExpr;

                if ((Token.Types)currentToken.Type == Token.Types.Multiplication) { 
                    Match (Token.Types.Multiplication);
                    arithmeticExpr = new ArithmeticExpr ("*", currentToken.Row, currentToken.Column);
                } else {
                    Match (Token.Types.Division);
                    arithmeticExpr = new ArithmeticExpr ("/", currentToken.Row, currentToken.Column);
                }

                arithmeticExpr.AddChild (Factor ());
                Expression expression = ExprMult ();

                if (expression.Name != null) {
                    arithmeticExpr.AddChild (expression);
                }

                return arithmeticExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                       (Token.Types)currentToken.Type == Token.Types.Subtraction ||
                       (Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return new ArithmeticExpr(null, currentToken.Row, currentToken.Column);
            }

            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row, 
                currentToken.Column);
        }

        private Expression Factor ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis) {
                Match (Token.Types.LeftParenthesis);
                Expression expr = Expr ();
                Match (Token.Types.RightParenthesis);

                return expr;
            } else if ((Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                       (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                       (Token.Types)currentToken.Type == Token.Types.Identifier ||
                       (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                return Value ();
            }

            throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row, 
                currentToken.Column);
        }

        private Expression Value ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.IntLiteral:
                    {
                        IntValueExpr expr = new IntValueExpr (currentToken.Lexeme, currentToken.Row, currentToken.Column);
                        Match (Token.Types.IntLiteral);
                        return expr;
                    }
                case Token.Types.StringLiteral:
                    {
                        StringValueExpr expr = new StringValueExpr (currentToken.Lexeme, currentToken.Row, currentToken.Column);
                        Match (Token.Types.StringLiteral);
                        return expr;
                    }
                case Token.Types.BoolLiteral:
                    {
                        BoolValueExpr expr = new BoolValueExpr (currentToken.Lexeme, currentToken.Row, currentToken.Column);
                        Match (Token.Types.BoolLiteral);
                        return expr;
                    }
                case Token.Types.Identifier:
                    {
                        IdentifierValueExpr expr = new IdentifierValueExpr (currentToken.Lexeme, currentToken.Row, currentToken.Column);
                        Match (Token.Types.Identifier);
                        return expr;
                    }
                default:
                    {
                        throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row,
                            currentToken.Column);
                    }
            }
        }

        private Type Type ()
        {
            Token token;
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Int:
                    token = Match (Token.Types.Int);
                    return new IntType ("Int", token.Row, token.Column);
                case Token.Types.String:
                    token = Match (Token.Types.String);
                    return new StringType ("String", token.Row, token.Column);
                case Token.Types.Bool:
                    token = Match (Token.Types.Bool);
                    return new BoolType ("Bool", token.Row, token.Column);
                default:
                    throw new SyntaxError ("invalid type " + currentToken.Type, currentToken.Row,
                        currentToken.Column);
            }
        }

        private Token Match (Token.Types type)
        {
            if ((Token.Types)currentToken.Type == type) {
                Token current = currentToken;
                ReadNextToken ();
                return current;
            }

            throw new SyntaxError ("invalid token " + currentToken.Type + ", expected: " + type, currentToken.Row,
                currentToken.Column);
        }

        private void ReadNextToken ()
        {
            currentToken = scanner.GetNextToken ();
            if ((Token.Types)currentToken.Type == Token.Types.ERROR) {
                throw new LexicalError ("malformed token \"" + currentToken.Lexeme + "\"", currentToken.Row,
                    currentToken.Column);
            }
        }

        private void SkipToNextStatement ()
        {
            while (true) {
                ReadNextToken ();
                Token.Types type = (Token.Types)currentToken.Type;
                if (type == Token.Types.EOS) {
                    break;
                }

                if (type == Token.Types.Semicolon) {
                    ReadNextToken ();
                    break;
                }
            }
        }
    }
}

