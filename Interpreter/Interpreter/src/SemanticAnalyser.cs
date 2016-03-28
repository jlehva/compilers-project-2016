using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class SemanticAnalyser : NodeVisitor
    {
        public Dictionary<string, Symbol> SymbolTable { get; private set; }

        public SemanticAnalyser ()
        {
            SymbolTable = new Dictionary<string, Symbol> ();
        }

        public void Run (Program program)
        {
            Visit (program);
        }

        public void Visit (Node node)
        {
            System.Console.WriteLine ("Visit Node");
        }

        public void Visit (Program node)
        {
            System.Console.WriteLine ("Visit Program");
            node.Accept (this);
        }

        public void Visit(Stmts node)
        {
            System.Console.WriteLine ("Visit Stmts DO STUFF");
        }

        public void Visit (AssertStmt node)
        {
            System.Console.WriteLine ("Visit AssertStmt DO STUFF");
        }

        public void Visit (AssignmentStmt node)
        {
            System.Console.WriteLine ("Visit AssignmentStmt DO STUFF");
        }

        public void Visit (ForStmt node)
        {
            System.Console.WriteLine ("Visit ForStmt DO STUFF");
        }

        public void Visit (IdentifierNameStmt node)
        {
            System.Console.WriteLine ("Visit IdentifierNameStmt DO STUFF");
        }

        public void Visit (PrintStmt node)
        {
            System.Console.WriteLine ("Visit PrintStmt DO STUFF");
        }

        public void Visit (ReadStmt node)
        {
            System.Console.WriteLine ("Visit ReadStmt DO STUFF");
        }

        public void Visit (VarDeclStmt node)
        {
            System.Console.WriteLine ("Visit VarDeclStmt DO STUFF");
        }

        public void Visit (VarStmt node)
        {
            System.Console.WriteLine ("Visit VarStmt DO STUFF");
        }

        public void Visit (ArithmeticExpr node)
        {
            System.Console.WriteLine ("Visit ArithmeticExpr DO STUFF");
        }

        public void Visit (BoolValueExpr node)
        {
            System.Console.WriteLine ("Visit BoolValueExpr DO STUFF");
        }

        public void Visit (Expression node)
        {
            System.Console.WriteLine ("Visit Expression DO STUFF");
        }

        public void Visit (IdentifierValueExpr node)
        {
            System.Console.WriteLine ("Visit IdentifierValueExpr");
        }

        public void Visit (IntValueExpr node)
        {
            System.Console.WriteLine ("Visit IntValueExpr");
        }

        public void Visit (LogicalExpr node)
        {
            System.Console.WriteLine ("Visit LogicalExpr");
        }

        public void Visit (NotExpr node)
        {
            System.Console.WriteLine ("Visit NotExpr");
        }

        public void Visit (RelationalExpr node)
        {
            System.Console.WriteLine ("Visit RelationalExpr");
        }

        public void Visit (StringValueExpr node)
        {
            System.Console.WriteLine ("Visit StringValueExpr");
        }

        public void Visit (BoolType node)
        {
            System.Console.WriteLine ("Visit BoolType");
        }

        public void Visit (IntType node)
        {
            System.Console.WriteLine ("Visit IntType");
        }

        public void Visit (StringType node)
        {
            System.Console.WriteLine ("Visit StringType");
        }
    }
}

