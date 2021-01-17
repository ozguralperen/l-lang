using System.Collections.Generic;
using llang.Core.lexer;
using llang.Core.virtualmachine;

namespace llang.Core.parser
{
    public enum ExpressionType
    {
        FUNCT_DECL,
        GET_OBJ,
        OBJECT,
        FUNCTION,
        IDENTIFIER,
        BOOL,
        STRING,
        INTEGER,
        ADD,
        SUBS,
        MUL,
        DIV,
        AND,
        OR,
        DISEQUAL,
        EQUAL,
        LESS,
        GREATER,
        LESS_OR_EQUAL,
        GREATER_OR_EQUAL
    }

    public class Expression
    {
        public ExpressionType kind;
        public ExpressionValue value;

        public List<string> AccessObj;

        public Expression(ExpressionType kind) { this.kind = kind; }
        public Expression(ExpressionType kind, ExpressionValue val) { this.kind = kind; this.value = val; }

    }

    public sealed class TokenExpression : Expression
    {
        public Token token;
        public TokenExpression(ExpressionType type, Token token) : base(type)
        {
            this.token = token;
        }
    }

    public sealed class ValueExpression : Expression
    {
        public ExpressionValue Value;
        public ValueExpression(ExpressionType type, ExpressionValue value) : base(type)
        {
            this.Value = value;
        }
    }

    public class FunctionExpression : Expression
    {
        public List<Expression> parameters;
        public string name;
        public Function function;

        public FunctionExpression(ExpressionType type, string id, List<Expression> parameters) : base(type)
        {
            this.name = id;
            this.parameters = parameters;
        }

        public FunctionExpression(ExpressionType type, Function fun) : base(type)
        {
            this.function = fun;
        }
        public FunctionExpression(ExpressionType type, List<string> accessor, List<Expression> parameters) :
        base(type)
        {
            this.parameters = parameters;
            this.AccessObj = accessor;
        }

    }
    public class AccessKeyExpression : Expression
    {
        public AccessKeyExpression(ExpressionType type, List<string> accessor) : base(type)
        {
            this.AccessObj = accessor;
        }
    }

    public class OperationExpression : Expression
    {
        public Expression rhs;
        public Expression lhs;
        public OperationExpression(ExpressionType type, Expression rhs, Expression lhs) : base(type)
        {
            this.rhs = rhs;
            this.lhs = lhs;
        }
    }

}