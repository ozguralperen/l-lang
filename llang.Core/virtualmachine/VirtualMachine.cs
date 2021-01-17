using System;
using llang.Core.parser;
using System.Collections.Generic;
using llang.Core.interpreter;

namespace llang.Core.virtualmachine
{
    public class VirtualMachine
    {
        private Environment env;
        private Evaluator evaluator;

        public Environment Environment
        {
            get
            {
                return this.env;
            }
        }

        public VirtualMachine()
        {
            this.env = new Environment();
            this.evaluator = new Evaluator(this);
        }

        public void Execute(Program program)
        {
            this.ExecuteStatements(program.Statements);
        }

        private ExpressionValue ExecuteStatements(Statements statements)
        {
            return this.ExecuteStatements(statements, this.env);
        }

        private ExpressionValue ExecuteStatements(Statements statements, Environment env)
        {
            ExpressionValue ret;

            for (int i = 0; i < statements.Length; i++)
                if ((ret = this.ExecuteStatement(statements.GetStatement(i), env)) != null)
                    return ret;

            return null;
        }

        private ExpressionValue ExecuteStatement(Statement statement)
        {
            return this.ExecuteStatement(statement, this.env);
        }

        private ExpressionValue ExecuteStatement(Statement statement, Environment env)
        {
            switch (statement.kind)
            {
                case StatementType.ASSIGN:
                    {
                        AssignmentStatement asg = statement as AssignmentStatement;
                        this.Assign(asg.assignment);
                        return null;
                    }
                case StatementType.IF:
                    {
                        IfStatement st = statement as IfStatement;
                        ExpressionValue value = this.evaluator.Evaluate(st.condition.Expression, this.env);
                        if (value.Bool == true)
                            return this.ExecuteStatements(st.body);
                        return null;
                    }
                case StatementType.IF_ELSE:
                    {
                        IfElseStatement st = statement as IfElseStatement;
                        ExpressionValue value = this.evaluator.Evaluate(st.condition.Expression, this.env);
                        if (value.Bool == true)
                            return this.ExecuteStatements(st.ifBody);
                        else
                            return this.ExecuteStatements(st.elseBody);
                    }
                case StatementType.WHILE:
                    {
                        WhileStatement st = statement as WhileStatement;
                        ExpressionValue value = this.evaluator.Evaluate(st.condition.Expression, this.env);
                        if (value.Bool == true)
                        {
                            Statements ss = new Statements();
                            ss.AddStatement(st.body);
                            ss.AddStatement(st);
                            return this.ExecuteStatements(ss);
                        }

                        return null;
                    }
                case StatementType.FUNC_DECL:
                    {
                        FunctionStatement st = statement as FunctionStatement;
                        this.DeclareFunction(st.function);
                        return null;
                    }
                case StatementType.FUNCTION:
                    {
                        FunctionStatement st = statement as FunctionStatement;
                        return this.ExecuteFunction(st.val);
                    }
                case StatementType.RETURN:
                    {
                        FunctionStatement st = statement as FunctionStatement;
                        return this.evaluator.Evaluate(st.val, this.Environment);
                    }
            }

            return null;
        }

        void DeclareFunction(Function function)
        {
            this.env.Declcare(
                function.Identifier,
                function
                );
        }

        public ExpressionValue ExecuteFunction(Expression _fun)
        {
            FunctionExpression fun = _fun as FunctionExpression;

            Function f = null;
            ExpressionValue obj = null;
            ExpressionValue last = null;

            if (fun.AccessObj == null)
            {
                f = this.env.Get(fun.name) as Function;
            }
            else
            {
                List<string> accessor = fun.AccessObj;

                obj = last = this.env.Get(accessor[0]) as ExpressionValue;

                for (int i = 1; i < accessor.Count; i++)
                {
                    last = obj;
                    obj = obj.GetProperty(accessor[i]);
                }

                f = obj.Function;
            }


            if (f == null)
            {
                if (this.IsSystemFunction(fun.name))
                {
                    return this.ExecuteSystemFunction(fun);
                }
                else
                    return null;
            }

            this.env.PushEnvironment();
            for (int i = 0; i < fun.parameters.Count; i++)
            {
                this.env.Declcare(
                    f.ParametersNames[i],
                    this.evaluator.Evaluate(fun.parameters[i], this.env)
                );
            }


            if (obj != null)
                this.env.Declcare("this", last);

            ExpressionValue ev = this.ExecuteStatements(f.InnerStatements);
            this.env.PopEnvironment();
            return ev;
        }

        private void Assign(Assignment assignment)
        {
            if (assignment.IsSimple)
            {
                if (assignment.IsGlobal)
                    this.env.Modify(
                        assignment.Variable,
                        this.evaluator.Evaluate(assignment.Value, this.env)
                    );
                else
                    this.env.Declcare(
                        assignment.Variable,
                        this.evaluator.Evaluate(assignment.Value, this.env)
                    );
            }
            else
            {
                List<string> accessor = assignment.AccesKey;
                ExpressionValue MainObject = this.env.Get(accessor[0]) as ExpressionValue;

                for (int i = 1; i < accessor.Count - 1; i++)
                {
                    MainObject = MainObject.GetProperty(accessor[i]);
                }

                MainObject.SetProperty(
                    accessor[accessor.Count - 1],
                    this.evaluator.Evaluate(assignment.Value, env)
                );
            }
        }

        bool IsSystemFunction(string functionName)
        {
            return (functionName == "print") || (functionName == "import");
        }

        private ExpressionValue ExecuteSystemFunction(Expression _fun)
        {
            FunctionExpression fun = _fun as FunctionExpression;
            if (fun.name == "print")
            {
				string str = "";
                ExpressionValue val = this.evaluator.Evaluate(fun.parameters[0], this.env);
                if (val.IsNumber)
                {
                    if (float.IsInfinity(val.Number))
                    {
                        str = "Infinity";
                    }
                    else if (float.IsNaN(val.Number))
                    {
                        str = "NaN";
                    }
                    else
                    {
                        str = val.Number.ToString();
                    }
                }
                else if (val.IsBool)
                    str = val.Bool.ToString();
                else if (val.IsString)
                    str = val.String;
                else if (val.IsFunction)
                    str = ("Function " + val.Function.Identifier);
                else if (val.IsObject)
                    str = "Object";
                else
                    str = "Undefined";

                

                if (fun.parameters.Count < 5 || fun.parameters.Count > 1)
                {
					List<string> vals = new List<string>();
                    for (int i = 1; i < fun.parameters.Count; i++)
                    {
                        ExpressionValue exp = this.evaluator.Evaluate(fun.parameters[i], this.env);

                        switch (i)
                        {
                            case 1: // font size
                                if (exp.IsNumber) vals.Add(exp.Number.ToString());
                                break;
                            case 2: // color
                                if (exp.IsString) vals.Add(exp.String);
                                break;
                            case 3: // top
                                if (exp.IsNumber) vals.Add(exp.Number.ToString());
                                break;
                            case 4: // left
                                if (exp.IsNumber) vals.Add(exp.Number.ToString());
                                break;
                            case 5: // font
                                if (exp.IsString) vals.Add(exp.String);
                                break;
							default:
								break;
                        }
                    }
					OutputFormatter op = new OutputFormatter(vals.ToArray() , str);	
					Console.WriteLine(op.ToString());
                }
				else
					Console.WriteLine(OutputFormatter.GetOnlyTag(str));

                return null;
            }
            else if (fun.name == "import")
            {
                if (fun.parameters.Count != 1)
                    return null;

                ExpressionValue fileName = this.evaluator.Evaluate(fun.parameters[0], this.env);

                if (!fileName.IsString)
                    return null;

                Interpreter i = Interpreter.FromFile(fileName.String);
                i.Init();
                ExpressionValue v = i.RunAsModule();
                return v;
            }
            else
                return null;
        }
    }
}