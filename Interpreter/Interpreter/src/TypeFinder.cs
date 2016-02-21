using System;
using System.Collections.Generic;

namespace Interpreter
{
	public class TypeFinder
	{
		// A limited set of operators include (only!) the ones listed below
		private static List<string> operators = new List<string> {
			"+", 
			"-", 
			"*", 
			"/", 
			"<", 
			"=", 
			"&", 
			"!"
		};
		private static List<string> types = new List<string> {
			"int",
			"string",
			"bool"
		};
		private static List<string> reserved_keywords = new List<string> {
			"var", 
			"for",
			"end",
			"in",
			"do",
			"read", 
			"print",
			"int",
			"string",
			"bool",
			"assert"
		};

		public TypeFinder ()
		{
		}
			
		public string findTypeFor(string lexeme) {
			if (operators.Contains(lexeme) {
				return "";
			}
			return "";
		}
	}
}

