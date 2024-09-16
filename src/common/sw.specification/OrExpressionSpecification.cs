using System;
using System.Linq.Expressions;

namespace sw.specification
{
    public class OrExpressionSpecification<T> : ExpressionSpecification<T>
    {
        private readonly ExpressionSpecification<T> _left;
        private readonly ExpressionSpecification<T> _right;

        public OrExpressionSpecification(ExpressionSpecification<T> left, ExpressionSpecification<T> right)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();
            //var paramExpr = Expression.Parameter(typeof(T));
            //var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            //exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            //var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression));

            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression));

            var visitor = new SwapVisitor(leftExpression.Parameters[0], rightExpression.Parameters[0]);
            var binaryExpression = Expression.OrElse(visitor.Visit(leftExpression.Body), rightExpression.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, rightExpression.Parameters);

            return lambda;

            //return finalExpr;
        }
    }
}