using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class InterpreterVisitor : NodeVisitor
    {
        public Dictionary<string, Symbol> SymbolTable { get; private set; }
        public Stack<StackValue> ValueStack { get; private set; }
        private Program program;

        public InterpreterVisitor (Program program)
        {
            SymbolTable = new Dictionary<string, Symbol> ();
            ValueStack = new Stack<StackValue> ();
            this.program = program;
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
            // Visit the expression of the Assert
            VisitChildren (node.Children [0]);
            // The result of the expression is then found from the ValueStack
            StackValue stackValue = ValueStack.Pop ();
            if (stackValue.Value == "false") { 
                throw new AssertError ("Assertion failed on row: " + node.Row);
            }
        }

        public void Visit (AssignmentStmt node)
        {
            string name = node.Children [0].Name;

            // visit the children, the value should be on top of the stack after that
            VisitChildren (node.Children [1]);
            StackValue stackValue = ValueStack.Pop ();
            // update the value in symbol table
            SymbolTable [name].Value = stackValue.Value;
        }

        public void Visit (ForStmt node)
        {
            Node identifierNameStmt = node.Children [0];
            Node startExpr = node.Children [1];
            Node endExpr = node.Children [2];
            Node statements = node.Children [3];

            VisitChildren (startExpr);
            int start = Int32.Parse (ValueStack.Pop ().Value);

            VisitChildren (endExpr);
            int end = Int32.Parse (ValueStack.Pop ().Value);

            for (int i = start; i <= end; i++) {
                SymbolTable [identifierNameStmt.Name].Value = i.ToString ();
                VisitChildren (statements);
            }
                
            ValueStack = new Stack<StackValue> (); // don't leave anything to stack after the For-loop
        }

        public void Visit (PrintStmt node)
        {
            VisitChildren (node);
            System.Console.WriteLine (ValueStack.Pop ().Value);
        }

        public void Visit (ReadStmt node)
        {
            Symbol variable = SymbolTable [node.Children [0].Name];
            string userInput = Console.ReadLine ().Trim ();

            if (variable.Type == "Int") {
                try {
                    Int32.Parse (userInput);
                } catch (Exception e) {
                    throw new RuntimeError ("Expected input value " + userInput + " to be integer");
                }
            }

            SymbolTable [variable.Name].Value = userInput;
        }

        public void Visit (VarDeclStmt node)
        {
            string name = node.Children [0].Name;
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

            // node.Children [2] is the value expression
            if (node.Children.Count == 3) {
                VisitChildren (node.Children [2]);
                value = ValueStack.Pop ().Value;
            }

            SymbolTable.Add (name, new Symbol (name, type, value));
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
            StackValue boolean = ValueStack.Pop ();

            // change true to false and false to true and push it to the stack
            if (boolean.Value == "true") {
                boolean.Value = "false";
            } else {
                boolean.Value = "true";
            }

            ValueStack.Push (boolean);
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
            ValueStack.Push (new StackValue("Bool", node.Name));
            VisitChildren (node);
        }

        public void Visit (StringValueExpr node)
        {
            ValueStack.Push (new StackValue("String", node.Name));
            VisitChildren (node);
        }

        public void Visit (IdentifierValueExpr node)
        {
            Symbol symbol = SymbolTable [node.Name];
            ValueStack.Push (new StackValue(symbol.Type, symbol.Value));
            VisitChildren (node);
        }

        public void Visit (IntValueExpr node)
        {               
            ValueStack.Push (new StackValue("Int", node.Name));
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
            StackValue value1 = ValueStack.Pop ();
            string op = node.Name;
            string opnd1 = value1.Value;
            string opnd2 = ValueStack.Pop ().Value;

            if (value1.Type == "Int") {
                CalcIntValues (op, opnd1, opnd2);
            } else if (value1.Type == "String") {
                CalcStringValues (op, opnd1, opnd2);
            } else if (value1.Type == "Bool") {
                CalcBoolValues (op, opnd1, opnd2);
            }
        }

        private void CalcIntValues(string op, string opnd1, string opnd2) {
            int int1 = Int32.Parse (opnd1);
            int int2 = Int32.Parse (opnd2);

            switch (op) {
                case "+":
                    ValueStack.Push (new StackValue("Int", int1 + int2));
                    return;
                case "-":
                    ValueStack.Push (new StackValue ("Int", int1 - int2));
                    return;
                case "*":
                    ValueStack.Push (new StackValue ("Int", int1 * int2));
                    return;
                case "/":
                    if (int2 == 0) {
                        throw new RuntimeError ("Division by zero is not allowed");
                    }
                    ValueStack.Push (new StackValue ("Int", int1 / int2));
                    return;
                case "=":
                    ValueStack.Push (new StackValue ("Bool", int1 == int2));
                    return;
                case "<":
                    ValueStack.Push (new StackValue ("Bool", int1 < int2));
                    return;
            }
        }

        private void CalcStringValues(string op, string string1, string string2) {
            switch (op) {
                case "+":
                    ValueStack.Push (new StackValue("String", string1 + string2));
                    return;
                case "=":
                    ValueStack.Push (new StackValue("Bool", string1 == string2));
                    return;
                case "<":
                    ValueStack.Push (new StackValue("Bool", string1.Length < string2.Length));
                    return;
            }
        }

        private void CalcBoolValues(string op, string opnd1, string opnd2) {
            bool bool1 = opnd1 == "true";
            bool bool2 = opnd2 == "true";

            switch (op) {
                case "=":
                    ValueStack.Push (new StackValue("Bool", bool1 == bool2));
                    return;
                case "<":
                    if (!bool1 && bool2) { // true only when false < true, "true" is considered "bigger than false"
                        ValueStack.Push (new StackValue ("Bool", true));
                    } else {
                        ValueStack.Push (new StackValue("Bool", false));
                    }

                    return;
                case "&":
                    ValueStack.Push (new StackValue("Bool", bool1 && bool2));
                    return;
            }
        }

    }
}

