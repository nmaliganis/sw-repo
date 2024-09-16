using System;
using System.Linq;
using System.Linq.Expressions;

namespace sw.specification
{
    public class NotExpressionSpecification<T> : ExpressionSpecification<T>
    {
        private readonly ExpressionSpecification<T> _left;

        public NotExpressionSpecification(ExpressionSpecification<T> left)
        {
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();

            var notExpression = Expression.Not(leftExpression.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(notExpression, leftExpression.Parameters.Single());

            return lambda;
        }
    }
}