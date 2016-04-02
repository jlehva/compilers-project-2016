using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

            if (parser.Errors.Count == 0 && semanticAnalyser.Errors.Count == 0) {
                interpreterVisitor.Run ();
            } else {
                List<Error> errors = parser.Errors;
                errors.AddRange (semanticAnalyser.Errors);
                errors = errors.OrderBy (e => e.Row).ThenBy (e => e.Column).ToList ();
                foreach (Error e in errors) {
                    System.Console.WriteLine (e.ToString ());
                }
            }
        }
    }
}
