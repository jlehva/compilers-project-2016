using System;

namespace Interpreter
{
    public interface NodeVisitor
    {
        void visit(Program node);
        void visit(AssertStmt node);
        void visit(AssignmentStmt node);
        void visit(ForStmt node);
        void visit(IdentifierNameStmt node);
        void visit(PrintStmt node);
        void visit(ReadStmt node);
        void visit(VarDeclStmt node);
        void visit(VarStmt node);
        void visit(ArithmeticExpr node);
        void visit(BoolValueExpr node);
        void visit(IdentifierValueExpr node);
        void visit(Expression node);
        void visit(IdentifierValueExpr node);
        void visit(IntValueExpr node);
        void visit(LogicalExpr node);
        void visit(NotExpr node);
        void visit(RelationalExpr node);
        void visit(StringValueExpr node);
        void visit(BoolType node);
        void visit(IntType node);
        void visit(StringType node);
    }
}

