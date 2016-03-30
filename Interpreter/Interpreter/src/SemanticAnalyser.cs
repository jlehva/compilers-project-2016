using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class SemanticAnalyser : NodeVisitor
    {
        public Dictionary<string, Symbol> SymbolTable { get; private set; }
        public Stack<Node> OperandStack { get; private set; }
        public Stack<Node> NodeStack { get; private set; }

        public SemanticAnalyser ()
        {
            SymbolTable = new Dictionary<string, Symbol> ();
            NodeStack = new Stack<Node> ();
        }

        public void Run (Program program)
        {
            program.Accept (this);

            System.Console.WriteLine ("- - - - - - - - - - - - - - - - - - - - -");
            System.Console.WriteLine ("NodeStack:");
            foreach (Node node in NodeStack) {
                System.Console.WriteLine (node.Name);
            }
            System.Console.WriteLine ("POPPING:");
            foreach (Node node in NodeStack) {
                System.Console.WriteLine (NodeStack.Pop ().Name);
            }
        }

        public void VisitChildren (Node node)
        {
            System.Console.WriteLine (node.GetType ());
            for (int i = 0; i < node.Children.Count - 1; i++) {
                node.Children[i].Accept (this);
            }

            if (node.Children.Count > 0) {
                node.Children[node.Children.Count - 1].Accept(this);
            }
        }

        public void Visit (Node node)
        {
            System.Console.WriteLine ("Visit Node");
        }

        public void Visit (Program node)
        {
            System.Console.WriteLine ("Visit Program");
            VisitChildren (node);
        }

        public void Visit(Stmts node)
        {
            System.Console.WriteLine ("Visit Stmts");
            VisitChildren (node);
        }

        public void Visit (AssertStmt node)
        {
            System.Console.WriteLine ("Visit AssertStmt DO STUFF");
            VisitChildren (node);
        }

        public void Visit (AssignmentStmt node)
        {
            System.Console.WriteLine ("Visit AssignmentStmt DO STUFF");
            VisitChildren (node);
        }

        public void Visit (ForStmt node)
        {
            System.Console.WriteLine ("Visit ForStmt DO STUFF");
            VisitChildren (node);
        }

        public void Visit (IdentifierNameStmt node)
        {
            System.Console.WriteLine ("Visit IdentifierNameStmt DO STUFF");
            VisitChildren (node);
        }

        public void Visit (PrintStmt node)
        {
            System.Console.WriteLine ("Visit PrintStmt DO STUFF");
            // NodeStack.Push (node);
            VisitChildren (node);
        }

        public void Visit (ReadStmt node)
        {
            System.Console.WriteLine ("Visit ReadStmt DO STUFF");
            // NodeStack.Push (node);
            VisitChildren (node);
        }

        public void Visit (VarDeclStmt node)
        {
            System.Console.WriteLine ("Visit VarDeclStmt DO STUFF");
            // NodeStack.Push (node);
            VisitChildren (node);
        }

        public void Visit (ArithmeticExpr node)
        {
            System.Console.WriteLine ("Visit ArithmeticExpr DO STUFF");
            if (node.Name != null) {
                NodeStack.Push (node);
            }
            VisitChildren (node);
        }

        public void Visit (BoolValueExpr node)
        {
            System.Console.WriteLine ("Visit BoolValueExpr DO STUFF");
            NodeStack.Push (node);
            VisitChildren (node);
        }

        public void Visit (Expression node)
        {
            System.Console.WriteLine ("Visit Expression DO STUFF");
            VisitChildren (node);
            // evaluate stack
        }

        public void Visit (IdentifierValueExpr node)
        {
            System.Console.WriteLine ("Visit IdentifierValueExpr");
            NodeStack.Push (node);
            VisitChildren (node);
        }

        public void Visit (IntValueExpr node)
        {
            System.Console.WriteLine ("Visit IntValueExpr " + node.Name);
            //f (node.Name != null) {
            NodeStack.Push (node);
            // }
                
            VisitChildren (node);
        }

        public void Visit (LogicalExpr node)
        {
            System.Console.WriteLine ("Visit LogicalExpr");
            if (node.Name != null) {
                NodeStack.Push (node);
            }
            VisitChildren (node);
        }

        public void Visit (NotExpr node)
        {
            System.Console.WriteLine ("Visit NotExpr");
            if (node.Name != null) {
                NodeStack.Push (node);
            }
            VisitChildren (node);
        }

        public void Visit (RelationalExpr node)
        {
            System.Console.WriteLine ("Visit RelationalExpr");
            if (node.Name != null) {
                NodeStack.Push (node);
            }
            VisitChildren (node);
        }

        public void Visit (StringValueExpr node)
        {
            System.Console.WriteLine ("Visit StringValueExpr");
            NodeStack.Push (node);
            VisitChildren (node);
        }

        public void Visit (BoolType node)
        {
            System.Console.WriteLine ("Visit BoolType");
            VisitChildren (node);
        }

        public void Visit (IntType node)
        {
            System.Console.WriteLine ("Visit IntType");
            VisitChildren (node);
        }

        public void Visit (StringType node)
        {
            System.Console.WriteLine ("Visit StringType");
            VisitChildren (node);
        }
    }
}

