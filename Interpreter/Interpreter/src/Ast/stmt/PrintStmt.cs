using System;

namespace Interpreter
{
    public class PrintStmt : Statement
    {
        public Expression PrintExpression { get; private set; }

        public PrintStmt (Expression printExpression, int row) : base (row)
        {
            PrintExpression = printExpression;
        }
    }
}

