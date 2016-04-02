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
            StreamReader charStream;
            string filePath;

            try {
                filePath = args [0];
            } catch (IndexOutOfRangeException) {
                Console.WriteLine ("Please give the file path as the first argument, such as ~/code.txt");
                return;
            }

            try {
                charStream = File.OpenText (filePath); 
            } catch (System.IO.FileNotFoundException) {
                Console.WriteLine("File \"" + filePath + "\" was not found. Make sure to give a proper file path as the argument.");
                return;
            }
                
            Parser parser = new Parser (new Scanner (charStream));
            Program program = parser.Parse ();
            SemanticAnalyser semanticAnalyser = new SemanticAnalyser (program);
            semanticAnalyser.Run ();
            InterpreterVisitor interpreterVisitor = new InterpreterVisitor (program);

            if (parser.Errors.Count == 0 && semanticAnalyser.Errors.Count == 0) {
                interpreterVisitor.Run ();
            } else {
                // Combine the lists of errors, sort them by row and column and print them to Console
                List<Error> errors = parser.Errors;
                errors.AddRange (semanticAnalyser.Errors);
                errors = errors.OrderBy (e => e.Row).ThenBy (e => e.Column).ToList ();

                foreach (Error e in errors) {
                    System.Console.WriteLine (e.Print ());
                }
            }
        }
    }
}
