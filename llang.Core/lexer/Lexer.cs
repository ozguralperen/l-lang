using System;
using System.Collections.Generic;

namespace llang.Core.lexer
{
	public class Lexer
	{
		private Source source;
		private List<Token> tokens;
		private Matcher matcher;
		private Token lastToken;

		private string CurrentLine;

		public List<Token> Tokens => this.tokens;

		private Lexer()
		{
			this.tokens = new List<Token> ();
			this.matcher = new Matcher ();
			this.lastToken = null;
		}

		private Lexer(Source src) : this()
		{
			this.source = src;
		}

		public Lexer(string scode) : this()
		{
			this.source = new Source (scode);
		}

		public static Lexer FromFile (string path)
		{
			Source src = Source.FromFile (path);
			return new Lexer (src);
		}

		private Token GetNextToken()
		{
			Token CurrentToken = this.matcher.Match (CurrentLine);

			if (CurrentToken == null)
				return null;

			int length = CurrentToken.Length;
			this.CurrentLine = this.CurrentLine.Substring (length);

			return CurrentToken;
		}

		private bool AddNextToken()
		{
			this.SkipEmptyChar ();

			if (this.CurrentLine.Length == 0)
				return false;

			Token CurrentToken = this.GetNextToken ();

			if (CurrentToken == null)
				return false;

			else if (this.lastToken != null && this.lastToken.Type == TokenType.MULTILINE_COMMENT_START) {
				if (CurrentToken.Type == TokenType.MULTILINE_COMMENT_END)
					this.lastToken = null;

				return true;
			} 
			else if (CurrentToken.Type == TokenType.INLINE_COMMENT)
				return false;
			else if (CurrentToken.Type == TokenType.MULTILINE_COMMENT_START) {
				this.lastToken = CurrentToken;
				return true;
			}

			tokens.Add (CurrentToken);

			if (CurrentToken.Type == TokenType.QUOTE) {
				string value = this.ParseString ();
				if (value == null)
					return false;

				this.tokens.Add (new Token (TokenType.ALPHANUMERIC, value));
				this.tokens.Add (CurrentToken);
			}

			this.lastToken = CurrentToken;
			return true;
		}

		private string ParseString ()
		{
			int ind = this.CurrentLine.IndexOf ('"');
			if (ind == -1)
				return null;

			string val = this.CurrentLine.Substring (0, ind);
			this.CurrentLine = this.CurrentLine.Substring (ind);

			if (this.CurrentLine [0] != '"')
				return null;

			this.CurrentLine = this.CurrentLine.Substring (1);
			return val;
		}

		private void SkipEmptyChar ()
		{
			int position = 0;

			while (position < this.CurrentLine.Length) {
				char c = this.CurrentLine [position];

				if (c == ' ' || c == '\t' || c == '\r')
					position++;
				else {
					this.CurrentLine = this.CurrentLine.Substring (position);
					break;
				}
			}
		}

		public void Tokenize() 
		{

			for (int i = 0; i < this.source.LineCount; i++) {
				this.CurrentLine = this.source.getLine ();

				while (this.AddNextToken());
			}
		}

		public void PrintToken(int index)
		{
			if (index < this.tokens.Count)
				Console.Write (this.tokens [index].ToString());
		}

		public void PrintToken()
		{
			for (int i = 0; i < this.tokens.Count; i++)
				this.PrintToken(i);
		}
	}
}