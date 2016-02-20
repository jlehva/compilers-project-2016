using System;
using System.IO;

namespace Interpreter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var filePath = args[0];
			StreamReader charStream = File.OpenText(filePath);
			Parser parser = new Parser (new Scanner(charStream));
			parser.parse ();
		}
	}
}
