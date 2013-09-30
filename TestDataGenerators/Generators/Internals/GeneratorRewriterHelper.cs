using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace TestDataGenerators
{
    public static class GeneratorRewriterHelper
    {
        public static ItemGeneratorRewriteResult<TItem, TValueGetter> AnalyzeAndRewriteItemGenerator<TItem, TValueGetter>(
            Expression<Func<int, TItem>> itemGenerator,
            Func<int, MethodCallExpression, string> parameterNameGenerator)
            where TValueGetter : IValueGetter
        {
            var valueGetterParameterExpression = Expression.Parameter(typeof(TValueGetter), "valueGetter");
            var dataList = new List<ParameterSpec>();
            var testExpressionVisitor = new ItemGeneratorExpressionVisitor(
                valueGetterParameterExpression,
                (key, possibleValues, node) => dataList.Add(new ParameterSpec(key, parameterNameGenerator(key, node), possibleValues)));

            var rewritedBodyItemGeneratorExpression = (Expression<Func<int, TItem>>)testExpressionVisitor.Visit(itemGenerator);
            var rewritedItemGeneratorExpression = Expression.Lambda<Func<int, TValueGetter, TItem>>(
                rewritedBodyItemGeneratorExpression.Body,
                rewritedBodyItemGeneratorExpression.Parameters.Concat(new[] { valueGetterParameterExpression }));

            return new ItemGeneratorRewriteResult<TItem, TValueGetter>(rewritedItemGeneratorExpression, dataList);
        }
    }
}