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
            VisitChildren (node.Children [1]);
        }

        public void Visit (ForStmt node)
        {
            Node identifierNameStmt = node.Children [0];
            Node startExpr = node.Children [1];
            Node endExpr = node.Children [2];
            Node statements = node.Children [3];

            if (!SymbolTable.ContainsKey (identifierNameStmt.Name)) {
                throw new SemanticError ("Semantic error: Symbol " + identifierNameStmt.Name + " needs to be declared before use, on row: " + identifierNameStmt.Row);
            }
                
            VisitChildren (startExpr);
            string type = TypeStack.Pop ();
            if (type != "Int") {
                throw new SemanticError ("Semantic error: For range needs to start with int value, not with " + type + ", on row: " + startExpr.Row);
            }

            VisitChildren (endExpr);
            type = TypeStack.Pop ();
            if (type != "Int") {
                throw new SemanticError ("Semantic error: For range needs to start with int value, not with " + type + ", on row: " + startExpr.Row);
            }

            VisitChildren (statements);
            TypeStack = new Stack<string> (); // clean the stack as there's no use for the type value(s) at this point
            System.Console.WriteLine ("TypeStack after visiting For statements, size: " + TypeStack.Count);
        }

        public void Visit (PrintStmt node)
        {
            VisitChildren (node);
            string type = TypeStack.Pop ();
            if (type != "Int" && type != "String") {
                throw new SemanticError ("Semantic error: Only String or Int values can be printed (tried to print value of type " +
                    type + ", on row: " + node.Children [0].Row);
            }
        }

        public void Visit (ReadStmt node)
        {
            if (!SymbolTable.ContainsKey (node.Children [0].Name)) {
                throw new SemanticError ("Semantic error: Symbol " + node.Children [0].Name + " must be declared before value " +
                    "can be assigned for it , on row: " + node.Children [0].Row);
            }
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
            System.Console.WriteLine ("TypeStack should be empty to begin with: " + TypeStack.Count);
            if (node.Children.Count == 3) {
                System.Console.WriteLine ("who is this: " + node.Children [2].Name);
                VisitChildren(node.Children [2]);
                System.Console.WriteLine ("TypeStack after visiting expression " + TypeStack.Count);
                string typeFromStack = TypeStack.Pop ();
                System.Console.WriteLine (typeFromStack);
                if (typeFromStack != type) {
                    throw new SemanticError ("Semantic error: Expression value assigned for " + name + 
                        " was not the same type of " + type + ", on row: " + node.Children [0].Row);
                }
            }
            System.Console.WriteLine ("TypeStack should be empty: " + TypeStack.Count);
            SymbolTable.Add (name, new Symbol(name, type, value));
            // VisitChildren (node);
        }

        public void Visit (ArithmeticExpr node)
        {
            System.Console.WriteLine ("Visit ArithmeticExpr " + node.Name);
            VisitChildren (node);
            ApplyOperatorToOperands (node);
        }

        public void Visit (Expression node)
        {
            VisitChildren (node);
        }

        public void Visit (LogicalExpr node)
        {
            System.Console.WriteLine ("Visit LogicalExpr");
            VisitChildren (node);
            ApplyOperatorToOperands (node);
        }

        public void Visit (NotExpr node)
        {
            VisitChildren (node);
        }

        public void Visit (RelationalExpr node)
        {
            System.Console.WriteLine ("Visit RelationalExpr");
            VisitChildren (node);
            ApplyOperatorToOperands (node);
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

        public void Visit (BoolType node)
        {
        }

        public void Visit (IntType node)
        {
        }

        public void Visit (StringType node)
        {
        }

        public void ApplyOperatorToOperands(Node node) {
            System.Console.WriteLine ("Applying " + node.Name + " to TypeStack of size: " + TypeStack.Count);
            string type1 = TypeStack.Pop ();
            string type2 = TypeStack.Pop ();

            switch (node.Name) {
                case "+":
                    if (type1 == "String" && type2 == "String") {
                        TypeStack.Push ("String");
                    } else {
                        ApplyToIntOperands (type1, type2, node);
                    }
                    return;
                case "-":
                    ApplyToIntOperands (type1, type2, node);
                    return;
                case "*":
                    ApplyToIntOperands (type1, type2, node);
                    return;
                case "/":
                    ApplyToIntOperands (type1, type2, node);
                    return;
                case "=":
                    if (type1 == type2) {
                        TypeStack.Push ("Bool");
                    } else {
                        throwOperatorError (type1, type2, node);
                    }
                    return;
                case "<":
                    if (type1 == type2) {
                        TypeStack.Push ("Bool");
                    } else {
                        throwOperatorError (type1, type2, node);
                    }
                    return;
                case "&":
                    if (type1 == "Bool" && type2 == "Bool") {
                        TypeStack.Push ("Bool");
                    } else {
                        throwOperatorError (type1, type2, node);
                    }
                    return;
            }
        }

        public void ApplyToIntOperands(string type1, string type2, Node node) {
            if (type1 == "Int" && type2 == "Int") {
                TypeStack.Push ("Int"); // "Int op Int" results to new Int in the stack
                System.Console.WriteLine ("pushed value to TypeStack, size now: " + TypeStack.Count);
            } else {
                throwOperatorError (type1, type2, node);
            }
        }
            
        public void throwOperatorError(string type1, string type2, Node node) {
            throw new SemanticError ("Semantic error: Operator " + node.Name + " can not be applied to operands of types: " +
                type1 + " & " + type2 + ", on row " + node.Children [0].Row);
        }
    }
}

