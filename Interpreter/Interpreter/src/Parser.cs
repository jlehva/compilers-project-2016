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
                Program program = new Program ("program", currentToken.Row);
                program.AddChild (Stmts ());
                return program;
            }

            throw new SyntaxError ("Syntax error: invalid start symbol of a program " + currentToken.Type + 
            " on row " + currentToken.Row + "and column " + currentToken.Column);
        }

        private Stmts Stmts ()
        {
            // System.Console.WriteLine (" = = = = = = = = = = NEW STATEMENT = = = = = ");
            Stmts statements = new Stmts ("stmts", currentToken.Row);

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
                } catch (Exception e) {
                    errors.Add (e);
                    SkipToNextStatement ();
                    return Stmts ();
                }
            } else if ((Token.Types)currentToken.Type == Token.Types.End ||
                       (Token.Types)currentToken.Type == Token.Types.EOS) {
                return statements;
            } 

            throw new SyntaxError ("Syntax error: invalid start symbol for statement " + currentToken.Lexeme + 
            " on row " + currentToken.Row + "and column " + currentToken.Column);
        }

        private Statement Stmt ()
        {
            switch ((Token.Types)currentToken.Type) {
                case Token.Types.Var:
                    VarDeclStmt varDeclStmt = new VarDeclStmt ("VarDecl", currentToken.Row);
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

                    throw new SyntaxError ("Expected Assign, got something else: " + currentToken.Type);
                case Token.Types.Identifier:
                    AssignmentStmt assignmentStmt = new AssignmentStmt ("AssignmentStmt", currentToken.Row);
                    assignmentStmt.AddChild (IdentifierNameStmt ());
                    Match (Token.Types.Assign);
                    assignmentStmt.AddChild (Expr ());
                    return assignmentStmt;
                case Token.Types.For:
                    ForStmt forStmt = new ForStmt ("ForStmt", currentToken.Row);
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
                    ReadStmt readStmt = new ReadStmt ("ReadStmt", currentToken.Row);
                    Match (Token.Types.Read);
                    readStmt.AddChild (IdentifierNameStmt ());
                    return readStmt;
                case Token.Types.Print:
                    PrintStmt printStmt = new PrintStmt ("PrintStmt", currentToken.Row);
                    Match (Token.Types.Print);
                    printStmt.AddChild (Expr ());
                    return printStmt;
                case Token.Types.Assert:
                    AssertStmt assertStmt = new AssertStmt ("AssertStmt", currentToken.Row);
                    Match (Token.Types.Assert);
                    Match (Token.Types.LeftParenthesis);
                    assertStmt.AddChild (Expr ());
                    Match (Token.Types.RightParenthesis);
                    return assertStmt;
                default:
                    throw new SyntaxError ("Syntax error: invalid start symbol for statement " + currentToken.Lexeme + 
                        " on row " + currentToken.Row + "and column " + currentToken.Column);
            }
        }

        private IdentifierNameStmt IdentifierNameStmt () 
        {
            Token token = Match (Token.Types.Identifier);
            IdentifierNameStmt identifierNameStmt = new IdentifierNameStmt(token.Lexeme, token.Row);
            return identifierNameStmt;
        }
            
        private Expression Expr ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - Expr ()");
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression expression = new Expression ("expr", currentToken.Row);

                Expression left = Conjuct ();
                Expression right = ExprLogical ();

                if (right.Name != null) {
                    right.AddChild (left);
                    expression.AddChild (right);
                    return expression;
                    // right.AddChild (left);
                    // return right;
                } else {
                    System.Console.WriteLine ("EKA EXPRESSION " + left.Children.Count);
                    return left;
                }
            } else if ((Token.Types)currentToken.Type == Token.Types.Not) {
                NotExpr notExpr = new NotExpr ("!", currentToken.Row);
                Match (Token.Types.Not);
                notExpr.AddChild (Expr ());
                return notExpr;
            }

            throw new SyntaxError ("Syntax Error: invalid token to start expression " + currentToken.Type + " on row " +
            currentToken.Row + "and column " + currentToken.Column);
            
        }

        private Expression ExprLogical ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - ExprLogical ()");
            if ((Token.Types)currentToken.Type == Token.Types.And) {
                LogicalExpr logicalExpr = new LogicalExpr ("&", currentToken.Row);
                Match (Token.Types.And);

                logicalExpr.AddChild (Conjuct ());
                Expression expression = ExprLogical ();
                if (expression.Name != null) {
                    logicalExpr.AddChild (expression);
                }
                // logicalExpr.AddChild (ExprLogical ());  
                return logicalExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return new LogicalExpr(null, currentToken.Row); // TODO, return new logical expression instead?
            }
                
            throw new SyntaxError ("Syntax Error: invalid token to start expression " + currentToken.Type + " on row " +
            currentToken.Row + "and column " + currentToken.Column);
        }

        private Expression Conjuct ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - Conjuct ()");
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression expression = new Expression ("expr", currentToken.Row);

                Expression left = ExprC ();
                Expression right = ExprRelational ();
                if (right.Name != null) {
                    right.AddChild (left);
                    expression.AddChild (right);
                    expression.Print ();
                    return expression;
                    //right.AddChild (left);
                    // return right;
                } else {
                    System.Console.WriteLine ("");
                    System.Console.WriteLine (" = = = = = = = Conjuct");
                    left.Print ();
                    return left;
                }
            }
                
            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprRelational ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - ExprRelational ()");
            if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.Less) {

                RelationalExpr relationalExpr;

                if ((Token.Types)currentToken.Type == Token.Types.Equal) {
                    Match (Token.Types.Equal);
                    relationalExpr = new RelationalExpr ("=", currentToken.Row);
                } else {
                    Match (Token.Types.Less);
                    relationalExpr = new RelationalExpr ("<", currentToken.Row);
                }

                relationalExpr.AddChild (ExprC ());
                Expression expression = ExprRelational ();
                if (expression.Name != null) {
                    relationalExpr.AddChild (expression);
                }
                // relationalExpr.AddChild (ExprRelational ());
                return relationalExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                return new RelationalExpr(null, currentToken.Row);
            }
             
            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprC ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - ExprC ()");
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                Expression expression = new Expression ("expr", currentToken.Row);

                Expression left = Term ();
                Expression right = ExprAdd ();
                if (right.Name != null) {
                    right.AddChild (left);
                    expression.AddChild (right);
                    System.Console.WriteLine (" = = = = = = = ExprC ASD");
                    System.Console.WriteLine ("expression " + expression.Name);
                    System.Console.WriteLine ("left " + left.Name);
                    System.Console.WriteLine ("right " + right.Name);
                    expression.Print ();
                    return expression;
                    //right.AddChild (left);
                    //return right;
                } else {
                    System.Console.WriteLine (" = = = = = = = ExprC LOL");
                    left.Print ();
                    return left;
                }
            }

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprAdd ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - ExprAdd ()");
            if ((Token.Types)currentToken.Type == Token.Types.Addition ||
                (Token.Types)currentToken.Type == Token.Types.Subtraction) {

                ArithmeticExpr arithmeticExpr;

                if ((Token.Types)currentToken.Type == Token.Types.Addition) {   
                    Match (Token.Types.Addition);
                    arithmeticExpr = new ArithmeticExpr ("+", currentToken.Row);
                } else {
                    Match (Token.Types.Subtraction);
                    arithmeticExpr = new ArithmeticExpr ("-", currentToken.Row);
                }
                System.Console.WriteLine ("EKA FRAGMENT");
                arithmeticExpr.Print ();
                arithmeticExpr.AddChild (Term ());
                System.Console.WriteLine ("TOKA FRAGMENT");
                arithmeticExpr.Print ();
                Expression expression = ExprAdd ();
                if (expression.Name != null) {
                    System.Console.WriteLine ("KOLMAS FRAGMENT");
                    arithmeticExpr.AddChild (expression);
                }
                // arithmeticExpr.AddChild (ExprAdd ());
                System.Console.WriteLine ("");
                System.Console.WriteLine (" = = = = = = = ExprAdd");
                arithmeticExpr.Print ();
                System.Console.WriteLine (" = = = = = = =");
                return arithmeticExpr;
            } else if ((Token.Types)currentToken.Type == Token.Types.Equal ||
                       (Token.Types)currentToken.Type == Token.Types.Less ||
                       (Token.Types)currentToken.Type == Token.Types.And ||
                       (Token.Types)currentToken.Type == Token.Types.RightParenthesis ||
                       (Token.Types)currentToken.Type == Token.Types.Range ||
                       (Token.Types)currentToken.Type == Token.Types.Do ||
                       (Token.Types)currentToken.Type == Token.Types.Semicolon) {
                System.Console.WriteLine ("null +- arithmeticExpr");
                return new ArithmeticExpr(null, currentToken.Row);
            }

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            " and column " + currentToken.Column);
        }

        private Expression Term ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - Term ()");
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis ||
                (Token.Types)currentToken.Type == Token.Types.IntLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Equal ||
                (Token.Types)currentToken.Type == Token.Types.StringLiteral ||
                (Token.Types)currentToken.Type == Token.Types.Identifier ||
                (Token.Types)currentToken.Type == Token.Types.BoolLiteral) {
                // Expression expression = new Expression ("expr", currentToken.Row);

                Expression left = Factor ();
                Expression right = ExprMult ();
                if (right.Name != null) {
                    // expression.AddChild (left);
                    // expression.AddChild (right);
                    // return expression;
                    right.AddChild (left);
                    right.Print ();
                    return right;
                } else {
                    left.Print ();
                    return left;
                }
            }
                
            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression ExprMult ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - ExprMult ()");
            if ((Token.Types)currentToken.Type == Token.Types.Multiplication ||
                (Token.Types)currentToken.Type == Token.Types.Division) {

                ArithmeticExpr arithmeticExpr;

                if ((Token.Types)currentToken.Type == Token.Types.Multiplication) { 
                    Match (Token.Types.Multiplication);
                    arithmeticExpr = new ArithmeticExpr ("*", currentToken.Row);
                } else {
                    Match (Token.Types.Division);
                    arithmeticExpr = new ArithmeticExpr ("/", currentToken.Row);
                }

                arithmeticExpr.AddChild (Factor ());
                Expression expression = ExprMult ();

                if (expression.Name != null) {
                    arithmeticExpr.AddChild (expression);
                }
                System.Console.WriteLine ("");
                System.Console.WriteLine (" = = = = = = = ExprMult");
                arithmeticExpr.Print ();
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
                System.Console.WriteLine ("null arithmeticExpr");
                return new ArithmeticExpr(null, currentToken.Row);
            }

            throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
            "and column " + currentToken.Column);
        }

        private Expression Factor ()
        {
            System.Console.WriteLine ("- - - - - - - - - - - - - Factor ()");
            if ((Token.Types)currentToken.Type == Token.Types.LeftParenthesis) {
                Match (Token.Types.LeftParenthesis);
                Expression expr = Expr ();
                Match (Token.Types.RightParenthesis);
                expr.Print ();
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
            System.Console.WriteLine ("- - - - - - - - - - - - - Value ()");
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
                    return new IntType ("Int", token.Row);
                case Token.Types.String:
                    token = Match (Token.Types.String);
                    return new StringType ("String", token.Row);
                case Token.Types.Bool:
                    token = Match (Token.Types.Bool);
                    return new BoolType ("Bool", token.Row);
                default:
                    throw new SyntaxError ("Syntax error: invalid type " + currentToken.Lexeme + " on row " + currentToken.Row +
                    "and column " + currentToken.Column);
            }
        }

        private Token Match (Token.Types type)
        {
            // System.Console.WriteLine (type);
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
            System.Console.WriteLine (currentToken.Lexeme);
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

