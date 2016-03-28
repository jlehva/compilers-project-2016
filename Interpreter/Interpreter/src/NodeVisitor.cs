using System;

namespace Interpreter
{
    public interface NodeVisitor
    {
        void Visit(Node node);
        void Visit(Program node);
        void Visit(Stmts node);
        void Visit(AssertStmt node);
        void Visit(AssignmentStmt node);
        void Visit(ForStmt node);
        void Visit(IdentifierNameStmt node);
        void Visit(PrintStmt node);
        void Visit(ReadStmt node);
        void Visit(VarDeclStmt node);
        void Visit(VarStmt node);
        void Visit(ArithmeticExpr node);
        void Visit(BoolValueExpr node);
        void Visit(Expression node);
        void Visit(IdentifierValueExpr node);
        void Visit(IntValueExpr node);
        void Visit(LogicalExpr node);
        void Visit(NotExpr node);
        void Visit(RelationalExpr node);
        void Visit(StringValueExpr node);
        void Visit(BoolType node);
        void Visit(IntType node);
        void Visit(StringType node);
    }
}

