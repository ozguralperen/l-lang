namespace llang.Core.parser
{

	public class Condition
	{
		private Expression expression;

		public Expression Expression {
			get {
				return this.expression;
			}
		}

		public Condition (Expression exp)
		{
			this.expression = exp;
		}
	}
	
}