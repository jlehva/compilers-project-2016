using System;
using System.Collections;
using System.Collections.Generic;

namespace Interpreter
{
	public class TypeFinder
	{
		private static Hashtable operators = new Hashtable {
			{"+", Token.Types.Addition},
			{"-", Token.Types.Subtraction}, 
			{"*", Token.Types.Multiplication},
			{"/", Token.Types.Division},
			{"<", Token.Types.Less},
			{"=", Token.Types.Equal},
			{"&", Token.Types.And},
			{"!", Token.Types.Not},
			{":=", Token.Types.Assign}
		};

		private static Hashtable types = new Hashtable {
			{"int", Token.Types.Int},
			{"string", Token.Types.String},
			{"bool", Token.Types.Bool}
		};

		private static Hashtable reserved_keywords = new Hashtable() {
			{"var", Token.Types.Var}, 
			{"for", Token.Types.For},
			{"end", Token.Types.End},
			{"in", Token.Types.In},
			{"do", Token.Types.Do},
			{"read", Token.Types.Read}, 
			{"print", Token.Types.Print},
			{"int", Token.Types.Int},
			{"string", Token.Types.String},
			{"bool", Token.Types.Bool},
			{"assert", Token.Types.Assert}
		};

		private static Hashtable symbols = new Hashtable() {
			{":", Token.Types.Colon},
			{";", Token.Types.Semicolon}
		};

		public TypeFinder ()
		{
		}
			
		public Token.Types findTypeFor(string lexeme) {
			if (reserved_keywords.ContainsKey (lexeme)) {
				return (Token.Types) reserved_keywords[lexeme];
			} else if (operators.ContainsKey (lexeme)) {
				return (Token.Types) operators[lexeme];
			} else if(symbols.ContainsKey(lexeme)) {
				return (Token.Types) symbols[lexeme];
			} else {
				return Token.Types.NONE;
			}

		}

	}
}

