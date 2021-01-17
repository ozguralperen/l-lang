using System.Collections.Generic;
using llang.Core.parser;

namespace llang.Core.virtualmachine
{
    public class Evaluator
	{
		private VirtualMachine vm;

		public Evaluator(VirtualMachine vm)
		{
			this.vm = vm;
		}

		public ExpressionValue Evaluate (Expression exp, Environment env)
		{
			if (exp == null)
				return new ExpressionValue (ExpressionValueType.BOOLEAN, false);

			switch (exp.kind) {
			case (ExpressionType.FUNCTION):
				{
					FunctionExpression ex = exp as FunctionExpression;
					return this.vm.ExecuteFunction (ex);
				}
			case (ExpressionType.FUNCT_DECL):
				{					
					FunctionExpression decl = exp as FunctionExpression;
					return new ExpressionValue (ExpressionValueType.FUNCTION, decl.function);
				}
			case (ExpressionType.OBJECT):
				{
					return new ExpressionValue (ExpressionValueType.OBJECT);
				}
			case (ExpressionType.GET_OBJ):
				{
                    AccessKeyExpression get = exp as AccessKeyExpression;
					List<string> accessor = get.AccessObj;
					ExpressionValue v = env.Get (accessor [0]) as ExpressionValue;

					for (int i = 1; i < accessor.Count; i++)
						v = v.GetProperty (accessor [i]);

					return v;
				}
			case (ExpressionType.IDENTIFIER):
				{
					TokenExpression tok = exp as TokenExpression;
					string id = tok.token.Value;
					return env.Get (id) as ExpressionValue;
				}
			case (ExpressionType.BOOL):
				{
					return exp.value;
				}
			case (ExpressionType.STRING):
				{
					return exp.value;
				}
			case (ExpressionType.INTEGER):
				{
					return exp.value;
				}
			case (ExpressionType.ADD):
				{
                    OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					if (v1.IsString)
						return new ExpressionValue (ExpressionValueType.STRING, v2.String + v1.String);
					else 
						return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number + v2.Number);
				}

			case (ExpressionType.SUBS):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number - v2.Number);
				}

			case (ExpressionType.MUL):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number * v2.Number);
				}
			case (ExpressionType.DIV):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number / v2.Number);
				}
			case (ExpressionType.AND):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Bool && v2.Bool);
				}
			case (ExpressionType.OR):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Bool || v2.Bool);
				}
			case (ExpressionType.EQUAL):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, 
					                            (v1.Bool == v2.Bool) && (v1.Number == v2.Number) && (v1.String == v2.String));
				}
			case (ExpressionType.DISEQUAL):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, 
					                            (v1.Bool != v2.Bool) && (v1.Number != v2.Number) && (v1.String != v2.String));
				}
				
			case (ExpressionType.LESS):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);


					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Number < v2.Number);
				}
				
			case (ExpressionType.GREATER):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number > v2.Number);
				}
				
			case (ExpressionType.LESS_OR_EQUAL):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);


					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number <= v2.Number);
				}
				
			case (ExpressionType.GREATER_OR_EQUAL):
				{
					OperationExpression op = exp as OperationExpression;
					ExpressionValue v1 = this.Evaluate (op.lhs, env);
					ExpressionValue v2 = this.Evaluate (op.rhs, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number >= v2.Number);
				}
			default:
				return null;
			}
		}

		public static float ToNumber (string value)
		{
			return float.Parse (value);
		}

		public static bool ToBool (string value)
		{
			return bool.Parse (value);
		}
	}
}