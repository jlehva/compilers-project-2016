using System;
using System.Collections;
using System.Collections.Generic;

namespace Interpreter
{
	public class TypeFinder
	{
		enum Operator { Addition, Subtraction, Multiplication, Division, Less, Equal, And, Not, Assign };
		// A limited set of operators include (only!) the ones listed below
		private static Hashtable operators = new Hashtable {
			{"+", Operator.Addition},
			{"-", Operator.Subtraction}, 
			{"*", Operator.Multiplication},
			{"/", Operator.Division},
			{"<", Operator.Less},
			{"=", Operator.Equal},
			{"&", Operator.And},
			{"!", Operator.Not},
			{":=", Operator.Assign}
		};

		enum Type { Int, String, Bool }

		private static Hashtable types = new Hashtable {
			{"int", Type.Int},
			{"string", Type.String},
			{"bool", Type.Bool}
		};

		enum Keyword { Var, For, End, In, Do, Read, Print, Int, String, Bool, Assert };

		private static Hashtable reserved_keywords = new Hashtable() {
			{"var", Keyword.Var}, 
			{"for", Keyword.For},
			{"end", Keyword.End},
			{"in", Keyword.In},
			{"do", Keyword.Do},
			{"read", Keyword.Read}, 
			{"print", Keyword.Print},
			{"int", Keyword.Int},
			{"string", Keyword.String},
			{"bool", Keyword.Bool},
			{"assert", Keyword.Assert}
		};

		enum Symbol { Colon, Quotation, Semicolon };

		private static Hashtable symbols = new Hashtable() {
			{":", Symbol.Colon},
			{"\"", Symbol.Quotation},
			{";", Symbol.Semicolon}
		};

		public TypeFinder ()
		{
		}
			
		public string findTypeFor(string lexeme) {
			if (operators.Contains(lexeme)) {
				return "";
			}
			return "";
		}
	}
}

