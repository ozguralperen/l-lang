namespace llang.Core.parser
{
    public enum StatementType
    {
        FUNCTION,
        FUNC_DECL,
        IF,
        IF_ELSE,
        WHILE,
        ASSIGN,
        RETURN
    };
    public class Statement
    {
        public StatementType kind;
        public Statement(StatementType kind) { this.kind = kind; }

        public override string ToString()
        {
            return kind.ToString();
        }
    }
    public class FunctionStatement : Statement
    {
        public Expression val;
        public FunctionStatement(StatementType type, Expression fun) : base(type)
        {
            if (type == StatementType.RETURN)
                this.val = fun;
            else
                this.val = fun;
        }
        public Function function;
        public FunctionStatement(StatementType type, Function fun) : base(type)
        {
            this.function = fun;
        }

    }
    public class IfStatement : Statement
    {
        public Condition condition;
        public Statements body;
        public IfStatement (StatementType type, Condition cond, Statements stat1) : base(type)
        {
            this.condition = cond;
            this.body = stat1;
        }
    }
    public class IfElseStatement : Statement
    {
        public Condition condition;
        public Statements ifBody;
        public Statements elseBody;
        public IfElseStatement(StatementType type, Condition cond, Statements stat1, Statements stat2) : base(type)
        {
            this.condition = cond;
            this.ifBody = stat1;
            this.elseBody = stat2;
        }
    }
    public class WhileStatement : Statement
    {
        public Condition condition;
        public Statements body;
        public WhileStatement(StatementType type, Condition cond, Statements stat1) : base(type)
        {
            this.condition = cond;
            this.body = stat1;
        }
    }

	public class AssignmentStatement : Statement
    {
        public Assignment assignment;
        public AssignmentStatement(StatementType type, Assignment assignment) : base(type)
        {
            this.assignment = assignment;
        }
    }
}