using System;

namespace Interpreter
{
    public class Error : Exception
    {
        public int Row { get; private set;}
        public int Column { get; private set;}

        public Error (string message, int row, int column) : base (message)
        {
            Row = row;
            Column = column;
        }

        public virtual string Print() {
            return Message;
        }
    }
}

