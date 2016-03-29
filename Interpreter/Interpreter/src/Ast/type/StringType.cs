using System;

namespace Interpreter
{
    public class StringType : Type
    {
        public StringType (string name, int row) : base(name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

