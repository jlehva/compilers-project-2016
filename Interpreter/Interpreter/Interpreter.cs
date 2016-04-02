using System;
using System.IO;

namespace Interpreter
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var filePath = args [0];
            StreamReader charStream = File.OpenText (filePath);
            Parser parser = new Parser (new Scanner (charStream));
            Program program = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (program);
            semanticAnalyser.Run ();
            InterpreterVisitor interpreterVisitor = new InterpreterVisitor (program);
            interpreterVisitor.Run ();
        }
    }
}
