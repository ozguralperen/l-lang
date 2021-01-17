using System;
using System.Collections.Generic;
using llang.Core.lexer;
using llang.Core.virtualmachine;

namespace llang.Core.parser
{
	public class Function 
	{
		private string Name;
		private List<string> Parameters;
		private Statements innerStatements;
	
		public string Identifier => Name;

		public List<string> ParametersNames => Parameters;

		public Statements InnerStatements => innerStatements;

		public Function(string name, Statements inner, List<string> parameters, Expression returnValue)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.innerStatements = inner;
		}
	}	
}