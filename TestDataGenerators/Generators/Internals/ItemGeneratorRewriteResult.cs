using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace TestDataGenerators
{
    public struct ItemGeneratorRewriteResult<TItem, TValueGetter>
        where TValueGetter : IValueGetter
    {
        public readonly Expression<Func<int, TValueGetter, TItem>> RewritedItemGeneratorExpression;
        public readonly List<ParameterSpec> DataList;

        public ItemGeneratorRewriteResult(Expression<Func<int, TValueGetter, TItem>> rewritedItemGeneratorExpression, List<ParameterSpec> dataList)
        {
            RewritedItemGeneratorExpression = rewritedItemGeneratorExpression;
            DataList = dataList;
        }
    }
}