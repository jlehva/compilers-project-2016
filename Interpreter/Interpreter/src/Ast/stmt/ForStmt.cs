using System;

namespace Interpreter
{
    public class ForStmt : Statement
    {
        public Expression Start { get; private set; }
        public Expression End { get; private set; }
        public Stmts DoStatements { get; private set; }

        public ForStmt (Expression start, Expression end, Stmts doStatements, int row) : base(row)
        {
            Start = start;
            End = end;
            DoStatements = doStatements;
        }
    }
}

