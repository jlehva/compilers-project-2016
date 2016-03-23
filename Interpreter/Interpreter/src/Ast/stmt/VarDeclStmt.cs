using System;

namespace Interpreter
{
    public class VarDeclStmt : Statement
    {
        private Type _type;
        private string _name;

        public VarDeclStmt (Type type, string name, int row) : base (row)
        {
            _type = type;
            _name = name;
        }

        public Type Type {
            get { return _type; }
            set { _type = value; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }
    }
}

