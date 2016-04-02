using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class SemanticAnalyser : NodeVisitor
    {
        public Dictionary<string, Symbol> SymbolTable { get; private set; }

        public Stack<string> TypeStack { get; private set; }

        private Program program;

        public List<Error> Errors { get; private set; }

        public SemanticAnalyser (Program program)
        {
            SymbolTable = new Dictionary<string, Symbol> ();
            TypeStack = new Stack<string> ();
            this.program = program;
            Errors = new List<Error> ();
        }

        public void Run ()
        {
            program.Accept (this);
        }

        public void VisitChildren (Node node)
        {
            for (int i = 0; i < node.Children.Count - 1; i++) {
                node.Children [i].Accept (this);
            }

            if (node.Children.Count > 0) {
                node.Children [node.Children.Count - 1].Accept (this);
            }
        }

        public void Visit (Node node)
        {
        }

        public void Visit (Program node)
        {
            VisitChildren (node);
        }

        public void Visit (Stmts node)
        {
            VisitChildren (node);
        }

        public void Visit (AssertStmt node)
        {
            try {
                VisitChildren (node.Children [0]);
                string type = TypeStack.Pop ();
                if (type != "Bool") {
                    throw new SemanticError ("Assert must be applied to expression that " +
                        "evaluates to Bool, evaluated to: " + type, node.Children [0].Row, node.Children [0].Column);
                }
            } catch (SemanticError error) {
                Errors.Add (error);
                TypeStack.Clear ();
            }
        }

        public void Visit (AssignmentStmt node)
        {
            try {
                string name = node.Children [0].Name;

                if (!SymbolTable.ContainsKey (name)) {
                    throw new SemanticError ("Variable " + name +
                    " needs to be declared before use", node.Children [0].Row,
                        node.Children [0].Column);
                }

                // check if the assigned expression is of the same type as the variable in symbol table
                VisitChildren (node.Children [1]);
                string type = TypeStack.Pop ();

                if (type != SymbolTable [name].Type) {
                    throw new SemanticError ("Variable " + name + " of type " + SymbolTable [name].Type +
                    " can not be assigned a value of type " + type, node.Children [0].Row, node.Children [0].Column);
                }
            } catch (SemanticError error) {
                Errors.Add (error);
                TypeStack.Clear ();
            }
        }

        public void Visit (ForStmt node)
        {
            try {
                Node identifierNameStmt = node.Children [0];
                Node startExpr = node.Children [1];
                Node endExpr = node.Children [2];
                Node statements = node.Children [3];

                if (!SymbolTable.ContainsKey (identifierNameStmt.Name)) {
                    throw new SemanticError ("Variable " + identifierNameStmt.Name +
                    " needs to be declared before use", identifierNameStmt.Row,
                    identifierNameStmt.Column);
                } else if (SymbolTable [identifierNameStmt.Name].Type != "Int") {
                    throw new SemanticError ("Variable " + identifierNameStmt.Name +
                    " must be of type int to be used in For-statement", identifierNameStmt.Row,
                    identifierNameStmt.Column);
                }
                
                VisitChildren (startExpr);
                string type = TypeStack.Pop ();
                if (type != "Int") {
                    throw new SemanticError ("For-range start needs to be int value, not " +
                    type, startExpr.Row, startExpr.Column);
                }

                VisitChildren (endExpr);
                type = TypeStack.Pop ();
                if (type != "Int") {
                    throw new SemanticError ("For-range end needs to be int value, not " +
                    type, endExpr.Row, endExpr.Column);
                }

                VisitChildren (statements);
            } catch (SemanticError error) {
                Errors.Add (error);
            }

            TypeStack.Clear (); // clean the stack as there's no use for the type value(s) at this point
        }

        public void Visit (PrintStmt node)
        {
            try {
                VisitChildren (node);
                string type = TypeStack.Pop ();
                if (type != "Int" && type != "String") {
                    throw new SemanticError ("Only String or Int values can be printed " +
                    "(tried to print value of type: " + type + ")", node.Children [0].Row, 
                        node.Children [0].Column);
                }
            } catch (SemanticError error) {
                Errors.Add (error);
                TypeStack.Clear ();
            }
        }

        public void Visit (ReadStmt node)
        {
            try {
                if (!SymbolTable.ContainsKey (node.Children [0].Name)) {
                    throw new SemanticError ("Variable " + node.Children [0].Name + " must be declared" +
                    " before it can be assigned a value", node.Children [0].Row,
                    node.Children [0].Column);
                }
            } catch (SemanticError error) {
                Errors.Add (error);
                TypeStack.Clear ();
            }
        }

        public void Visit (VarDeclStmt node)
        {
            try {
                string name = node.Children [0].Name;

                if (SymbolTable.ContainsKey (name)) {
                    throw new SemanticError ("Variable with name " + name + " is already defined",
                    node.Children [0].Row, node.Children [0].Column);
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

                SymbolTable.Add (name, new Symbol (name, type, value));

                // node.Children [2] is the value expression, check that the types match
                if (node.Children.Count == 3) {
                    VisitChildren (node.Children [2]);
                    string typeFromStack = TypeStack.Pop ();

                    if (typeFromStack != type) {
                        throw new SemanticError ("Expression value assigned for " + name +
                        " was not the same type of " + type, node.Children [2].Row,
                        node.Children [2].Column);
                    }
                }

            } catch (SemanticError error) {
                Errors.Add (error);
                TypeStack.Clear ();
            }
        }

        public void Visit (ArithmeticExpr node)
        {
            VisitChildren (node);
            ApplyOperatorToOperands (node);
        }

        public void Visit (Expression node)
        {
            VisitChildren (node);
        }

        public void Visit (LogicalExpr node)
        {
            VisitChildren (node);
            ApplyOperatorToOperands (node);
        }

        public void Visit (NotExpr node)
        {
            VisitChildren (node.Children [0]);
            string type = TypeStack.Pop ();
            if (type != "Bool") {
                throw new SemanticError ("Operator " + node.Name + " can not be applied to operand of type: " +
                    type, node.Children [0].Row, node.Children [0].Column);
            }
        }

        public void Visit (RelationalExpr node)
        {
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
            try {
                TypeStack.Push (SymbolTable [node.Name].Type);
                VisitChildren (node);
            } catch (KeyNotFoundException e) {
                throw new SemanticError ("Variable with name " + node.Name + " is not defined",
                    node.Row, node.Column);
            }
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

        public void ApplyOperatorToOperands (Node node)
        {
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

        public void ApplyToIntOperands (string type1, string type2, Node node)
        {
            if (type1 == "Int" && type2 == "Int") {
                TypeStack.Push ("Int"); // "Int op Int" results to new Int in the stack
            } else {
                throwOperatorError (type1, type2, node);
            }
        }

        public void throwOperatorError (string type1, string type2, Node node)
        {
            throw new SemanticError ("Operator " + node.Name + " can not be applied to operands of types: " +
            type1 + " & " + type2, node.Children [0].Row, node.Children [0].Column);
        }
    }
}
