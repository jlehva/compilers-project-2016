using System;

namespace Interpreter
{
    public class StackValue
    {
        public string Type { get; private set;}
        public string Value { get; set;}

        public StackValue (string type, string value)
        {
            Type = type;
            Value = value;
        }

        public StackValue (string type, bool value)
        {
            Type = type;

            if (value) {
                Value = "true";
            } else {
                Value = "false";
            }
        }

        public StackValue (string type, int value)
        {
            Type = type;
            Value = value.ToString ();
        }

        public StackValue (string type, double value)
        {
            Type = type;
            int temp = (int) Math.Round(value);
            Value = temp.ToString ();
        }
    }
}

