using System;
using System.Collections.Generic;

namespace Interpreter
{
    public abstract class Node
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string Name { get; set; } // mainly used for printing the tree
        public List<Node> Children { get; private set; }

        public Node (string name, int row, int column)
        {
            Name = name;
            Row = row;
            Column = column;
            Children = new List<Node>();
        }

        public Node (Token token) {
            Name = token.Lexeme;
            Row = token.Row;
            Column = token.Column;
        }

        public void AddChild(Node child) {
            Children.Add (child);
        }

        public abstract void Accept (NodeVisitor visitor);

        // http://stackoverflow.com/questions/4965335/how-to-print-binary-tree-diagram/8948691#8948691
        public void Print() {
            Print("", true);
        }

        private void Print(String prefix, bool isTail) {
            System.Console.WriteLine(prefix + (isTail ? "└── " : "├── ") + this.Name + " (" + this.GetType () + ")");
            for (int i = 0; i < Children.Count - 1; i++) {
                Children[i].Print(prefix + (isTail ? "    " : "│   "), false);
            }
            if (Children.Count > 0) {
                Children[Children.Count - 1].Print(prefix + (isTail ?"    " : "│   "), true);
            }
        }
    }
}

