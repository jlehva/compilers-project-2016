using System;

namespace Interpreter
{
    public class Symbol
    {
        public string Name { get; private set;}
        public string Type { get; private set;}
        public string Value { get; set;}

        public Symbol (string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}

