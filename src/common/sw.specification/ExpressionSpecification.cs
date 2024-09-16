using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace sw.specification
{
    public abstract class ExpressionSpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();
        public ExpressionSpecification<T> And(ExpressionSpecification<T> specification) => new AndExpressionSpecification<T>(this, specification);
        public ExpressionSpecification<T> Or(ExpressionSpecification<T> specification) => new OrExpressionSpecification<T>(this, specification);
        public ExpressionSpecification<T> Not(ExpressionSpecification<T> specification) => new NotExpressionSpecification<T>(this);

        public virtual bool IsSatisfiedBy(T candidate)
        {
            Func<T, bool> predicate = ToExpression().Compile();

            return predicate(candidate);
        }

        //public abstract bool IsSatisfiedBy(T candidate);

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression from, to;

            public SwapVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == from ? to : base.Visit(node);
            }
        }
    }
}
