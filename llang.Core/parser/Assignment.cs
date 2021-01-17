using System.Threading;
using System;
using System.Collections.Generic;
using llang.Core.lexer;
using llang.Core.virtualmachine;

namespace llang.Core.parser
{
	public class Assignment
	{
		private string variable;
		private Expression value;
		private bool global;
		private bool simple;
		private List<string> accesKey;

		public bool IsGlobal => this.global;

		public bool IsSimple => this.simple;
		public string Variable  => this.variable;

		public Expression Value  => this.value;

		public List<string> AccesKey => accesKey;

		public Assignment (string variable, bool global)
		{
			this.variable = variable;
			this.global = global;
			this.simple = true;
		}

		public Assignment (string variable, Expression value, bool global) : this(variable, global)
		{
			this.value = value;
		}

		public Assignment (List<string> accessor, Expression value, bool global)
		{
			this.accesKey = accessor;
			this.value = value;
			this.global = global;
			this.simple = false;
		}
	}
}