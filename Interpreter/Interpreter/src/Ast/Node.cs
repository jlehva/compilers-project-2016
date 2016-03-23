using System;

namespace Interpreter
{
    public abstract class Node
    {
        private int _row;

        public Node (int row)
        {
            _row = row;
        }

        public int Row {
            get { return _row; }
            set { _row = value; }
        }
    }
}

