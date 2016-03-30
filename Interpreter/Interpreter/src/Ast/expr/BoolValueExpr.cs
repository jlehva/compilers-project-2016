﻿using System;

namespace Interpreter
{
    public class BoolValueExpr : Expression
    {
        public BoolValueExpr (string name, int row) : base (name, row)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

