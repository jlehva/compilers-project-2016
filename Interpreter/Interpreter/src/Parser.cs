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
            PrintErrors ();
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
                AddError ("Invalid start symbol: " + currentToken.Lexeme);
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
                AddError ("Invalid statement: " + currentToken.Type);
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
            } else {
                throw new SyntaxError ("Expected Assign, got something else: " + currentToken.Type);
            }
        }

        private Expression Expr ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression left = Conjuct ();
                Expression right = ExprLogical ();

                if (right == null) {
                    return left;
                } else {
                    return new 
                }
            } else if ((Token.Types)currentToken.Type == Token.Types.Not) {
                Token not = Match (Token.Types.Not);
                return new UnaryExpr(new NotExpr(not.Row), Expr (), not.Row);
            } else {
                throw new SyntaxError ("Syntax Error: invalid token to start expression " + currentToken.Type + " on row " + 
                    currentToken.Row + "and column " + currentToken.Column);
            }
        }

        private Expression ExprLogical ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.And) {
                LogicalOp ();
                Conjuct ();
                ExprLogical ();
            } else if ((Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return null;
            } else {
                throw new SyntaxError ("Syntax Error: invalid token to start expression " + currentToken.Type + " on row " + 
                    currentToken.Row + "and column " + currentToken.Column);
            }
        }

        private void Conjuct ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                ExprC ();
                ExprRelational ();
            } else {
                AddError ("error");
            }
        }

        private void ExprRelational ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.Less) {
                RelationalOp ();
                ExprC ();
                ExprRelational ();
            } else if ((Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return;
            } else {
                AddError ("error");
            }
        }

        private void ExprC ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Term ();
                ExprAdd ();
            } else {
                AddError ("error");
            }
        }

        private void ExprAdd ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                (Token.Types)currentToken.Type == Token.Types.Subtraction) {
                AddOp ();
                Term ();
                ExprAdd ();
            } else if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return;
            } else {
                AddError ("error");
            }
        }

        private void Term ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Factor ();
                ExprMult ();
            } else {
                AddError ("error");
            }
        }

        private void ExprMult ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Multiplication ||
                (Token.Types)currentToken.Type == Token.Types.Division) {
                MultOp ();
                Factor ();
                ExprMult ();
            } else if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                       (Token.Types)currentToken.Type == Token.Types.Subtraction ||
                       (Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return;
            } else {
                AddError ("error");
            }
        }

        private void Factor ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis) {
                Match (Token.Types.LeftParenthesis);
                Expr ();
                Match (Token.Types.RightParenthesis);
            } else if ((Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                       (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                       (Token.Types)currentToken.Type == Token.Types.Identifier ||
                       (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Value ();
            }
        }

        private void Value ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.IntLiteral:
                    {
                        Match (Token.Types.IntLiteral);
                        return;
                    }
                case Token.Types.StringLiteral:
                    {
                        Match (Token.Types.StringLiteral);
                        return;
                    }
                case Token.Types.BoolLiteral:
                    {
                        Match (Token.Types.BoolLiteral);
                        return;
                    }
                case Token.Types.Identifier:
                    {
                        Match (Token.Types.Identifier);
                        return;
                    }
                default:
                    {
                        AddError ("Invalid operand: " + currentToken.Type);
                        return;
                    }
            }
        }

        private void LogicalOp ()
        {
            Match (Token.Types.And);
        }

        private void RelationalOp ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Equal) {
                Match (Token.Types.Equal);
            } else if ((Token.Types)currentToken.Type == Token.Types.Less) {
                Match (Token.Types.Less);
            }
        }

        private void AddOp ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Addition) {   
                Match (Token.Types.Addition);
            } else if ((Token.Types)currentToken.Type == Token.Types.Subtraction) {
                Match (Token.Types.Subtraction);
            }
        }

        private void MultOp ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Multiplication) { 
                Match (Token.Types.Multiplication);
            } else if ((Token.Types)currentToken.Type == Token.Types.Division) {
                Match (Token.Types.Division);
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
            } else {
                throw new SyntaxError ("Syntax Error: invalid token " + currentToken.Type + " on row " + currentToken.Row +
                "and column " + currentToken.Column + ", expected: " + type);
            }
        }

        private void ReadNextToken ()
        {
            currentToken = scanner.GetNextToken ();
        }

        private void PrintErrors ()
        {
            
        }

        private void AddError (string error)
        {
            errors.Add (new Exception (error));
        }

        private void SkipToNextStatement ()
        {
            while (true) {
                ReadNextToken ();
                if ((Token.Types)currentToken.Type == Token.Types.Var ||
                    (Token.Types)currentToken.Type == Token.Types.Identifier ||
                    (Token.Types)currentToken.Type == Token.Types.For ||
                    (Token.Types)currentToken.Type == Token.Types.Read ||
                    (Token.Types)currentToken.Type == Token.Types.Print ||
                    (Token.Types)currentToken.Type == Token.Types.Assert ||
                    (Token.Types)currentToken.Type == Token.Types.End ||
                    (Token.Types)currentToken.Type == Token.Types.EOS) {
                    break;
                }
            }
        }
    }
}

