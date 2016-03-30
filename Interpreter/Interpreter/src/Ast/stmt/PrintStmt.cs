using System;

namespace Interpreter
{
    public class PrintStmt : Statement
    {
        public PrintStmt (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

