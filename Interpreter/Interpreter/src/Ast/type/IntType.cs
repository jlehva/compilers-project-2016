using System;

namespace Interpreter
{
    public class IntType : Type
    {
        public IntType (string name, int row) : base(name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

