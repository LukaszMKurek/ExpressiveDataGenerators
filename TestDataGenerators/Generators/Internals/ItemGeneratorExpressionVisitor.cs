using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace ExpressiveDataGenerators
{
    /// <summary>
    /// Notifi found data source. Rewrite data source definition to value getter expression.
    /// </summary>
    internal sealed class ItemGeneratorExpressionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _valueGetterParameter;
        private readonly Action<int, IEnumerable, MethodCallExpression> _onFoundDataSource;
        private int _currentKey = -1;

        public ItemGeneratorExpressionVisitor(
            ParameterExpression valueGetterParameter,
            Action<int, IEnumerable, MethodCallExpression> onFoundDataSource)
        {
            _valueGetterParameter = valueGetterParameter;
            _onFoundDataSource = onFoundDataSource;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if ((node.Method.Name == "Of" && node.Method.DeclaringType == typeof(One)) == false) // todo rozwarzyć przeciążone
                return node;

            _currentKey += 1;

            var possibleValues = (IEnumerable)Expression.Lambda<Func<object>>(node.Arguments[0]).Compile()();

            _onFoundDataSource(_currentKey, possibleValues, node);

            var parameterNameConstant = Expression.Constant(_currentKey);

            MethodCallExpression callValueGetterExpression = Expression.Call(
                _valueGetterParameter,
                "GetValue",
                new[] { node.Arguments[0].Type.GetElementType() },
                new Expression[] { parameterNameConstant });

            return callValueGetterExpression;
        }
    }
}