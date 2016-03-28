using System;

namespace Interpreter
{
    public class Symbol
    {
        public string Name { get; private set;}
        public string Type { get; private set;}

        public Symbol (string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}

