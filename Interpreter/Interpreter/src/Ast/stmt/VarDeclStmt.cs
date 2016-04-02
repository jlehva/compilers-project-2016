﻿using System;

namespace Interpreter
{
    public class VarDeclStmt : Statement
    {
        public VarDeclStmt (string name, int row, int column) : base (name, row, column)
        {
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}

