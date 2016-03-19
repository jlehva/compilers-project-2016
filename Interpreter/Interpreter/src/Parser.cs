using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class Parser
    {
        private Scanner scanner;
        private Token currentToken;
        private List<Exception> errors = new List<Exception>();

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

        public List<Exception> GetErrors() {
            return errors;
        }

        private void Prog ()
        {
            // “var”, var_ident, “for”, “read”, “print”, “assert”
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
                    AddError ("Incorrect start symbol");
                    return;
            }
        }

        private void Stmts ()
        {
        }

        private void StmtsTail ()
        {
        }

        private void Stmt ()
        {
        }

        private void Assign ()
        {
        }

        private void Expr ()
        {
        }

        // A'
        private void ExprLogical ()
        {
        }

        // B
        private void ExprB ()
        {
        }

        // B'
        private void ExprComparison ()
        {
        }

        // C
        private void ExprC ()
        {
        }

        // C'
        private void ExprAdd ()
        {
        }

        // D
        private void ExprD ()
        {
        }

        // D'
        private void ExprMult ()
        {
        }

        // E
        private void ExprE ()
        {
        }

        private void Opnd ()
        {
        }

        private void Logicalop ()
        {
        }

        private void Comparisonop ()
        {
        }

        private void Addop ()
        {
        }

        private void Multop ()
        {
        }

        private void Type ()
        {
        }

        private void ReadNextToken ()
        {
            currentToken = scanner.GetNextToken ();
        }

        private void PrintErrors() 
        {
            
        }

        private void AddError(string error) {
            errors.Add (new Exception(error));
        }
    }
}

