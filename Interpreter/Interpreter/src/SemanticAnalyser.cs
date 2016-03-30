using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class SemanticAnalyser : NodeVisitor
    {
        public Dictionary<string, Symbol> SymbolTable { get; private set; }
        public Stack<Node> OperandStack { get; private set; }
        public Stack<string> TypeStack { get; private set; }

        public SemanticAnalyser ()
        {
            SymbolTable = new Dictionary<string, Symbol> ();
            TypeStack = new Stack<string> ();
        }

        public void Run (Program program)
        {
            program.Accept (this);

            System.Console.WriteLine ("- - - - - - - - - - - - - - - - - - - - -");
            System.Console.WriteLine ("TypeStack:");
            foreach (string type in TypeStack) {
                System.Console.WriteLine (type);
            }
        }

        public void VisitChildren (Node node)
        {
            System.Console.WriteLine (node.Name);
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

            try {
                if (!SymbolTable.ContainsKey (name)) {}
            } catch (KeyNotFoundException e) {
                throw new SemanticError ("Semantic error: Symbol with name " + name + " needs to be declared before use, on row: " + node.Children [0].Row);
            }
            // todo: check if the assigned expression is of the same type as the variable in symbol table
            VisitChildren (node);
        }

        public void Visit (ForStmt node)
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
                VisitChildren(node.Children [2]);
                if (TypeStack.Pop () != type) {
                    throw new SemanticError ("Semantic error: Expression value assigned for " + name + 
                        " was not the same type of " + type + ", on row: " + node.Children [0].Row);
                }
            }

            SymbolTable.Add (name, new Symbol(name, type, value));
            VisitChildren (node);
        }

        public void Visit (ArithmeticExpr node)
        {
            VisitChildren (node);
            OnlyIntValuesInExpression (node);
        }

        public void Visit (Expression node)
        {
            VisitChildren (node);
        }

        public void Visit (LogicalExpr node)
        {
            System.Console.WriteLine ("Visit LogicalExpr");
            VisitChildren (node);
        }

        public void Visit (NotExpr node)
        {
            VisitChildren (node);
        }

        public void Visit (RelationalExpr node)
        {
            VisitChildren (node);
            OnlyIntValuesInExpression (node);
        }

        public void Visit (IdentifierNameStmt node)
        {
            VisitChildren (node);
        }

        public void Visit (BoolValueExpr node)
        {
            TypeStack.Push ("Bool");
            VisitChildren (node);
        }

        public void Visit (StringValueExpr node)
        {
            TypeStack.Push ("String");
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

        public void Visit (BoolType node)
        {
        }

        public void Visit (IntType node)
        {
        }

        public void Visit (StringType node)
        {
        }

        public void OnlyIntValuesInExpression(Node node) {
            string type1 = TypeStack.Pop ();
            string type2 = TypeStack.Pop ();

            if (type1 != "Int" || type2 != "Int") {
                throw new SemanticError ("Semantic error: Operator " + node.Name + " can only be applied to Integers, not " +
                    type1 + " " + node.Name + " " + type2 + ", on row " + node.Children [0].Row);
            } else {
                TypeStack.Push ("Int"); // "Int op Int" results to new Int in the stack
            }
        }
    }
}

