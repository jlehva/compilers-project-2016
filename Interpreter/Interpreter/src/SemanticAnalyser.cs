using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class SemanticAnalyser : NodeVisitor
    {
        public Dictionary<string, Symbol> SymbolTable { get; private set; }
        public Stack<Node> OperandStack { get; private set; }
        public Stack<Node> NodeStack { get; private set; }
        public Stack<string> TypeStack { get; private set; }

        public SemanticAnalyser ()
        {
            SymbolTable = new Dictionary<string, Symbol> ();
            NodeStack = new Stack<Node> ();
            TypeStack = new Stack<string> ();
        }

        public void Run (Program program)
        {
            program.Accept (this);

            System.Console.WriteLine ("- - - - - - - - - - - - - - - - - - - - -");
            System.Console.WriteLine ("NodeStack:");
            foreach (Node node in NodeStack) {
                System.Console.WriteLine (node.Name);
            }
            System.Console.WriteLine ("TypeStack:");
            foreach (string type in TypeStack) {
                System.Console.WriteLine (type);
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
        }

        public void Visit (Program node)
        {
            VisitChildren (node);
        }

        public void Visit(Stmts node)
        {
            VisitChildren (node);
        }

        public void Visit (AssertStmt node)
        {
            VisitChildren (node);
        }

        public void Visit (AssignmentStmt node)
        {
            string name = node.Children [0].Name;

            if (!SymbolTable.ContainsKey (name)) {
                throw new SemanticError ("Semantic error: Symbol with name " + name + " needs to be declared before use, on row: " + node.Children [0].Row);
            }
            // todo: check if the assigned expression is of the same type as the variable in symbol table
            VisitChildren (node);
        }

        public void Visit (ForStmt node)
        {
            VisitChildren (node);
        }

        public void Visit (IdentifierNameStmt node)
        {
            VisitChildren (node);
        }

        public void Visit (PrintStmt node)
        {
            VisitChildren (node);
        }

        public void Visit (ReadStmt node)
        {
            VisitChildren (node);
        }

        public void Visit (VarDeclStmt node)
        {
            System.Console.WriteLine ("- - - - - - Visit VarDeclStmt");
            System.Console.WriteLine (node.Name);
            System.Console.WriteLine (node.Children.Count);
            System.Console.WriteLine (node.Children[0].Name);
            System.Console.WriteLine (node.Children[1].Name);
            // System.Console.WriteLine (node.Children[2].Name);
            string name = node.Children [0].Name;

            if (SymbolTable.ContainsKey (name)) {
                throw new SemanticError ("Semantic error: Symbol with name " + name + " already exists, on row: " + node.Children [0].Row);
            }

            string type = node.Children [1].Name;
            string value;

            //  If not explicitly initialized, variables are assigned an appropriate default value.
            if (type == "Int") {
                value = "0";
            } else if (type == "Bool") {
                value = "false";
            } else {
                value = null;
            }

            if (node.Children.Count == 3) {
                // value = evaluateExpressionValue (node.Children [2]);
            }

            SymbolTable.Add (name, new Symbol(name, type, value));

            VisitChildren (node);
        }

        public void Visit (ArithmeticExpr node)
        {
            VisitChildren (node);

            string type1 = TypeStack.Pop ();
            string type2 = TypeStack.Pop ();

            if (type1 != "Int" || type2 != "Int") {
                throw new SemanticError ("Semantic error: Operator " + node.Name + " can only be applied to Integers, not " +
                type1 + " " + node.Name + " " + type2 + ", on row " + node.Children [0].Row);
            } else {
                TypeStack.Push ("Int"); // "Int op Int" results to new Int in the stack
            }
        }

        public void Visit (BoolValueExpr node)
        {
            TypeStack.Push ("Bool");
            VisitChildren (node);
        }

        public void Visit (Expression node)
        {
            VisitChildren (node);
        }

        public void Visit (IdentifierValueExpr node)
        {
            TypeStack.Push (SymbolTable[node.Name].Type);
            VisitChildren (node);
        }

        public void Visit (IntValueExpr node)
        {               
            TypeStack.Push ("Int");
            VisitChildren (node);
        }

        public void Visit (LogicalExpr node)
        {
            System.Console.WriteLine ("Visit LogicalExpr");
            VisitChildren (node);
        }

        public void Visit (NotExpr node)
        {
            System.Console.WriteLine ("Visit NotExpr");
            VisitChildren (node);
        }

        public void Visit (RelationalExpr node)
        {
            System.Console.WriteLine ("Visit RelationalExpr");
            VisitChildren (node);
        }

        public void Visit (StringValueExpr node)
        {
            TypeStack.Push ("String");
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

