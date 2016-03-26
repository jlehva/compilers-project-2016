using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class Parser
    {
        private Scanner scanner;
        private Token currentToken;
        private List<Exception> errors = new List<Exception> ();

        public Parser (Scanner scanner)
        {
            this.scanner = scanner;
        }

        public Program Parse ()
        {
            ReadNextToken ();
            Program program = Prog ();
            // PrintErrors ();
            return program;
        }

        public List<Exception> GetErrors ()
        {
            return errors;
        }

        private Program Prog ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Var ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.For ||
                (Token.Types)currentToken.Type == Token.Types.Read ||
                (Token.Types)currentToken.Type == Token.Types.Print ||
                (Token.Types)currentToken.Type == Token.Types.Assert) {
                return new Program (Stmts (), currentToken.Row);
            } else {
                // AddError ("Invalid start symbol: " + currentToken.Lexeme);
                return new Program (currentToken.Row);
            }
        }

        private Stmts Stmts ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Var ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.For ||
                (Token.Types)currentToken.Type == Token.Types.Read ||
                (Token.Types)currentToken.Type == Token.Types.Print ||
                (Token.Types)currentToken.Type == Token.Types.Assert) {
                Statement left = Stmt ();
                Match (Token.Types.Semicolon);
                Stmts right = Stmts ();
                return new Stmts (left, right, currentToken.Row);
            } else if ((Token.Types)currentToken.Type == Token.Types.End ||
                       (Token.Types)currentToken.Type == Token.Types.EOS) {
                return new Stmts (currentToken.Row); // end of statements
            } else {
                // AddError ("Invalid statement: " + currentToken.Type);
                SkipToNextStatement ();
                return Stmts ();
            }
        }

        private Statement Stmt ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Var:
                    // VarDeclStmt
                    Match (Token.Types.Var);
                    Token variable = Match (Token.Types.Identifier);
                    Match (Token.Types.Colon);
                    Type type = Type ();
                    VarDeclStmt varDeclaration = new VarDeclStmt (type, variable.Lexeme, variable.Row);
                    return Assign (varDeclaration);
                case Token.Types.Identifier:
                    Match (Token.Types.Identifier);
                    Match (Token.Types.Assign);
                    Expr ();
                    return new Statement (currentToken.Row); // TODO  
                case Token.Types.For:
                    Match (Token.Types.For);
                    Match (Token.Types.Identifier);
                    Match (Token.Types.In);
                    Expr ();
                    Match (Token.Types.Range);
                    Expr ();
                    Match (Token.Types.Do);
                    Stmts ();
                    Match (Token.Types.End);
                    Match (Token.Types.For);
                    return new Statement (currentToken.Row); // TODO
                case Token.Types.Read:
                    Match (Token.Types.Read);
                    Match (Token.Types.Identifier);
                    return new Statement (currentToken.Row); // TODO  
                case Token.Types.Print:
                    Match (Token.Types.Print);
                    Expr ();
                    return new Statement (currentToken.Row); // TODO
                case Token.Types.Assert:
                    Match (Token.Types.Assert);
                    Match (Token.Types.LeftParenthesis);
                    Expr ();
                    Match (Token.Types.RightParenthesis);
                    return new Statement (currentToken.Row); // TODO   
                default:
                    throw new SyntaxError ("temp message");
            }
        }

        private Statement Assign (VarDeclStmt varDeclStmt)
        {
            if ((Token.Types)currentToken.Type == Token.Types.Assign) {
                Token assign = Match (Token.Types.Assign);
                return new AssignmentStmt (varDeclStmt, Expr (), assign.Row);
            } else if ((Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return varDeclStmt;
            }
             
            throw new SyntaxError ("Expected Assign, got something else: " + currentToken.Type);

        }

        private Expression Expr ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression left = Conjuct ();
                Expression right = ExprLogical ();
                return new Expression (left, right, t.Row);
            } else if ((Token.Types)currentToken.Type == Token.Types.Not) {
                Match (Token.Types.Not);
                return new NotExpr (Expr (), null, t.Row);
            }

            throw new SyntaxError ("Syntax Error: invalid token to start expression " + currentToken.Type + " on row " +
            currentToken.Row + "and column " + currentToken.Column);
            
        }

        private Expression ExprLogical ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.And) {
                Match (Token.Types.And);
                Expression left = Conjuct ();
                Expression right = ExprLogical ();

                if (right == null) {
                    return left;
                }
                    
                return new LogicalExpr (left, right, t.Row);
            } else if ((Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return null;
            }
                
            throw new SyntaxError ("Syntax Error: invalid token to start expression " + currentToken.Type + " on row " +
            currentToken.Row + "and column " + currentToken.Column);
        }

        private Expression Conjuct ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression left = ExprC ();
                Expression right = ExprRelational ();
                return new Expression (left, right, t.Row);
            }
                
            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprRelational ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.Less) {

                if ((Token.Types)currentToken.Type == Token.Types.Equal) {
                    Match (Token.Types.Equal);
                } else if ((Token.Types)currentToken.Type == Token.Types.Less) {
                    Match (Token.Types.Less);
                }

                Expression left = ExprC ();
                Expression right = ExprRelational ();
                return new RelationalExpr (left, right, t.Row, t.Lexeme);
            } else if ((Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return null;
            }
             
            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprC ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression left = Term ();
                Expression right = ExprAdd ();
                return new Expression (left, right, t.Row);
            }

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprAdd ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                (Token.Types)currentToken.Type == Token.Types.Subtraction) {

                if ((Token.Types)currentToken.Type == Token.Types.Addition) {   
                    Match (Token.Types.Addition);
                } else if ((Token.Types)currentToken.Type == Token.Types.Subtraction) {
                    Match (Token.Types.Subtraction);
                }

                Expression left = Term ();
                Expression right = ExprAdd ();
                return new ArtihmeticExpr (left, right, t.Row, t.Lexeme);
            } else if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return null;
            }

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression Term ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression left = Factor ();
                Expression right = ExprMult ();
                return new Expression (left, right, t.Row);
            }
                
            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprMult ()
        {
            Token t = currentToken;

            if ((Token.Types)currentToken.Type == Token.Types.Multiplication ||
                (Token.Types)currentToken.Type == Token.Types.Division) {

                if ((Token.Types)currentToken.Type == Token.Types.Multiplication) { 
                    Match (Token.Types.Multiplication);
                } else if ((Token.Types)currentToken.Type == Token.Types.Division) {
                    Match (Token.Types.Division);
                }

                Expression left = Factor ();
                Expression right = ExprMult ();
                return new ArtihmeticExpr (left, right, t.Row, t.Lexeme);
            } else if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                       (Token.Types)currentToken.Type == Token.Types.Subtraction ||
                       (Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return null;
            }

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
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

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression Value ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.IntLiteral:
                    {
                        IntValueExpr expr = new IntValueExpr (currentToken.Lexeme, currentToken.Row);
                        Match (Token.Types.IntLiteral);
                        return expr;
                    }
                case Token.Types.StringLiteral:
                    {
                        StringValueExpr expr = new StringValueExpr (currentToken.Lexeme, currentToken.Row);
                        Match (Token.Types.StringLiteral);
                        return expr;
                    }
                case Token.Types.BoolLiteral:
                    {
                        BoolValueExpr expr = new BoolValueExpr (currentToken.Lexeme, currentToken.Row);
                        Match (Token.Types.BoolLiteral);
                        return expr;
                    }
                case Token.Types.Identifier:
                    {
                        IdentifierValueExpr expr = new IdentifierValueExpr (currentToken.Lexeme, currentToken.Row);
                        Match (Token.Types.Identifier);
                        return expr;
                    }
                default:
                    {
                        throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
                        "and column " + currentToken.Column);
                    }
            }
        }

        private Type Type ()
        {
            Token token;
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Int:
                    token = Match (Token.Types.Int);
                    return new IntType (token.Row);
                case Token.Types.String:
                    token = Match (Token.Types.String);
                    return new StringType (token.Row);
                case Token.Types.Bool:
                    token = Match (Token.Types.Bool);
                    return new BoolType (token.Row);
                default:
                    throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
                    "and column " + currentToken.Column);
            }
        }

        private Token Match (Token.Types type)
        {
            System.Console.WriteLine (type);
            if ((Token.Types)currentToken.Type == type) {
                Token current = currentToken;
                ReadNextToken ();
                return current;
            } else if ((Token.Types)currentToken.Type == Token.Types.ERROR) {
                throw new LexicalError ("Lexical Error: malformed token \"" + currentToken.Lexeme + "\" on row " + currentToken.Row +
                "and column " + currentToken.Column);
            }

            throw new SyntaxError ("Syntax Error: invalid token " + currentToken.Type + " on row " + currentToken.Row +
            "and column " + currentToken.Column + ", expected: " + type);
        }

        private void ReadNextToken ()
        {
            currentToken = scanner.GetNextToken ();
        }

        private void SkipToNextStatement ()
        {
            while (true) {
                ReadNextToken ();
                Token.Types type = (Token.Types)currentToken.Type;
                if (type == Token.Types.Var ||
                    type == Token.Types.Identifier ||
                    type == Token.Types.For ||
                    type == Token.Types.Read ||
                    type == Token.Types.Print ||
                    type == Token.Types.Assert ||
                    type == Token.Types.End ||
                    type == Token.Types.EOS) {
                    break;
                }
            }
        }
    }
}

