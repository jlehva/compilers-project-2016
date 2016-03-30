﻿using System;

namespace Interpreter
{
    public class LogicalExpr : Expression
    {
        public LogicalExpr (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
