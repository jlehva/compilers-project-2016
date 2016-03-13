using System;

namespace Interpreter
{
    public class Parser
    {
        private Scanner scanner;

        public Parser (Scanner scanner)
        {
            this.scanner = scanner;
        }

        public void parse ()
        {
            this.scanner.getNextToken ();
        }
    }
}

