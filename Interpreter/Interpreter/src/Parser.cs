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

        public void Parse ()
        {
            ReadNextToken ();
            Prog ();
            PrintErrors ();
        }

        public List<Exception> GetErrors ()
        {
            return errors;
        }

        private void Prog ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Var:
                    Stmts ();
                    return;
                case Token.Types.Identifier:
                    Stmts ();
                    return;
                case Token.Types.For:
                    Stmts ();
                    return;
                case Token.Types.Read:
                    Stmts ();
                    return;
                case Token.Types.Print:
                    Stmts ();
                    return;
                case Token.Types.Assert:
                    Stmts ();
                    return;
                default:
                    AddError ("Invalid start symbol: " + currentToken.Type);
                    return;
            }
        }

        private void Stmts ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Var ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.For ||
                (Token.Types)currentToken.Type == Token.Types.Read ||
                (Token.Types)currentToken.Type == Token.Types.Print ||
                (Token.Types)currentToken.Type == Token.Types.Assert) {
                Stmt ();
                Match (Token.Types.Semicolon);
                Stmts ();
            } else if ((Token.Types)currentToken.Type == Token.Types.End ||
                (Token.Types)currentToken.Type == Token.Types.EOS) {
                // end of for or EOS
            } else {
                AddError ("Invalid symbol: " + currentToken.Type);
            }
        }

        private void Stmt ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Var:
                    {
                        Match (Token.Types.Var);
                        Match (Token.Types.Identifier);
                        Match (Token.Types.Colon);
                        Type ();
                        Assign ();
                        return;
                    }
                case Token.Types.Identifier:
                    {
                        Match (Token.Types.Identifier);
                        Match (Token.Types.Assign);
                        Expr ();
                        return;   
                    }
                case Token.Types.For:
                    {
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
                        return;
                    }
                case Token.Types.Read:
                    {
                        Match (Token.Types.Read);
                        Match (Token.Types.Identifier);
                        return;   
                    }
                case Token.Types.Print:
                    {
                        Match (Token.Types.Print);
                        Expr ();
                        return;
                    }
                case Token.Types.Assert:
                    {
                        Match (Token.Types.Assert);
                        Match (Token.Types.LeftParenthesis);
                        Expr ();
                        Match (Token.Types.RightParenthesis);
                        return;   
                    }
                default:
                    {
                        AddError ("Invalid start symbol: " + currentToken.Type);
                        return;
                    }
            }
        }

        private void Assign ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Assign) {
                Match (Token.Types.Assign);
                Expr ();
            } else if ((Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return;
            } else {
                AddError ("Expected Assign, got something else: " + currentToken.Type);
            }
        }

        private void Expr ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                ExprB ();
                ExprLogical ();
            } else if ((Token.Types)currentToken.Type == Token.Types.Not) {
                Match (Token.Types.Not);
                Expr ();
            } else {
                AddError ("Expected Expr, got something else: " + currentToken.Type);
            }
        }
            
        private void ExprLogical ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.And) {
                Logicalop ();
                ExprB ();
                ExprLogical ();
            } else if ((Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.Range ||
                (Token.Types)currentToken.Type == Token.Types.Do ||
                (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return;
            } else {
                AddError ("error");
            }
        }
            
        private void ExprB ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                ExprC ();
                ExprComparison ();
            } else {
                AddError ("error");
            }
        }
            
        private void ExprComparison ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.Less) {
                Comparisonop ();
                ExprC ();
                ExprComparison ();
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
                ExprD ();
                ExprAdd ();
            } else {
                AddError ("error");
            }
        }
            
        private void ExprAdd ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                (Token.Types)currentToken.Type == Token.Types.Subtraction) {
                Addop ();
                ExprD ();
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
            
        private void ExprD ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                ExprE ();
                ExprMult ();
            } else {
                AddError ("error");
            }
        }
            
        private void ExprMult ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Multiplication ||
                (Token.Types)currentToken.Type == Token.Types.Division) {
                Multop ();
                ExprE ();
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
            
        private void ExprE ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis) {
                Match (Token.Types.LeftParenthesis);
                Expr ();
                Match (Token.Types.RightParenthesis);
            } else if ((Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Opnd ();
            }
        }

        private void Opnd ()
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

        private void Logicalop ()
        {
            Match (Token.Types.And);
        }

        private void Comparisonop ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Equal) {
                Match (Token.Types.Equal);
            } else if ((Token.Types)currentToken.Type == Token.Types.Less) {
                Match (Token.Types.Less);
            }
        }

        private void Addop ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Addition) {   
                Match (Token.Types.Addition);
            } else if ((Token.Types)currentToken.Type == Token.Types.Subtraction) {
                Match (Token.Types.Subtraction);
            }
        }

        private void Multop ()
        {
            if ((Token.Types)currentToken.Type == Token.Types.Multiplication) { 
                Match (Token.Types.Multiplication);
            } else if ((Token.Types)currentToken.Type == Token.Types.Division) {
                Match (Token.Types.Division);
            }
        }

        private void Type ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Int:
                    {
                        Match (Token.Types.Int);
                        return;
                    }
                case Token.Types.String:
                    {
                        Match (Token.Types.String);
                        return;
                    }
                case Token.Types.Bool:
                    {
                        Match (Token.Types.Bool);
                        return;
                    }
                default:
                    {
                        AddError ("Invalid type: " + currentToken.Type);
                        return;
                    }
            }
        }

        private void Match(Token.Types type) {
            System.Console.WriteLine (type);
            if ((Token.Types)currentToken.Type == type) {
                ReadNextToken ();
            } else {
                AddError ("error");
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
    }
}

