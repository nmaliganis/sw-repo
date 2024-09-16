using System;
using System.Linq.Expressions;

namespace sw.specification
{
    public class AndExpressionSpecification<T> : ExpressionSpecification<T>
    {
        private readonly ExpressionSpecification<T> _left;
        private readonly ExpressionSpecification<T> _right;

        public AndExpressionSpecification(ExpressionSpecification<T> left, ExpressionSpecification<T> right)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            //BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

            //return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());

            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression));

            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression));

            var visitor = new SwapVisitor(leftExpression.Parameters[0], rightExpression.Parameters[0]);
            var binaryExpression = Expression.AndAlso(visitor.Visit(leftExpression.Body), rightExpression.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, rightExpression.Parameters);
            return lambda;
        }
    }
}