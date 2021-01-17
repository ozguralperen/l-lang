namespace llang.Core.parser
{
	public class Program
	{
		private Statements statements;

		public int Length => statements.Length;

		public Statements Statements => statements;

		public Program ()
		{
			this.statements = new Statements ();
		}

		public Program (Statements statements)
		{
			this.statements = statements;
		}

		public void AddStatement (Statements stat)
		{
			this.statements.AddStatement (stat);

		}

		public void AddStatement (Statement stat)
		{
			this.statements.AddStatement (stat);
		}

		public Statement GetStatement (int index)
		{
			return this.statements.GetStatement (index);
		}

		public void Print ()
		{
			this.statements.Print ();
		}
	}
}