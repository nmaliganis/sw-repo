using System.Linq.Expressions;

namespace sw.specification
{
    public sealed class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        protected override Expression VisitParameter(ParameterExpression node)
            => base.VisitParameter(_parameter);

        internal ParameterReplacer(ParameterExpression parameter)
        {
            this._parameter = parameter;
        }
    }
}