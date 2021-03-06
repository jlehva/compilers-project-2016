﻿using System;

namespace Interpreter
{
    public class LexicalError : Error
    {
        public LexicalError (string message, int row, int column) : base (message, row, column)
        {
        }

        public override string Print() {
            return "Lexical Error: " + Message + " [row: " + Row + ", col: " + Column + "]";
        }
    }
}

