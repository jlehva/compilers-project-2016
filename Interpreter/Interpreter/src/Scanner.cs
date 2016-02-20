using System;
using System.IO;
using System.Text;

namespace Interpreter
{
	public class Scanner
	{
		private int row = 0;
		private int column = 0;
		private StreamReader charStream;

		// used for tests
		public Scanner(string input) {
			byte[] byteArray = Encoding.UTF8.GetBytes(input);
			MemoryStream stream = new MemoryStream(byteArray);
			StreamReader charStream = new StreamReader(stream);
			this.charStream = charStream;
		}

		public Scanner(StreamReader charStream) {
			this.charStream = charStream;
		}

		public Token getNextToken() {
			return new Token();
		}

		public char getNextChar() {
			return (char)charStream.Read();
		}
	}
}

