using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestDataGenerators
{
   /// <summary>
   ///    Generate test case seqences.
   /// </summary>
   public static class Generate // todo jak rozszeżać? partial wypali?
   {
      /// <summary>
      ///    Generate cartesian product of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> AllCombination<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, Utils.CartesianProduct);
      }

      /// <summary>
      /// Generate random tuple of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> Random<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, Utils.Random);
      }

      /// <summary>
      /// Generate sequential tuple of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> Sequence<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, Utils.Sequence);
      }

      /// <summary>
      /// Generate infinitive sequential tuple of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> SequenceInfinitive<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, Utils.InfinitiveSequence);
      }

      /// <summary>
      /// Generate sequential tuple of values specisied in One.Of(...). Each One.Of must define equals numbers of elements.
      /// </summary>
      public static IEnumerable<TItem> SequenceStrict<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, Utils.StrictSequence);
      }

      private static IEnumerable<TItem> GenerateSequenceFrom<TItem>(
         Expression<Func<int, TItem>> itemGeneratorExpression,
         Func<IEnumerable<IEnumerable<object>>, IEnumerable<IEnumerable<object>>> sequenceGenerator)
      {
         if (itemGeneratorExpression == null)
            throw new ArgumentNullException("itemGeneratorExpression");

         GeneratorAnalizerHelper.ValidateItemGeneratorExpression(itemGeneratorExpression.Body);

         ItemGeneratorRewriteResult<TItem, ValueGetter> result =
            GeneratorRewriterHelper.AnalyzeAndRewriteItemGenerator<TItem, ValueGetter>(itemGeneratorExpression, (key, expr) => "");
         Func<int, ValueGetter, TItem> rewritedItemGenerator = result.RewritedItemGeneratorExpression.Compile();

         IEnumerable<IEnumerable<object>> sequence = sequenceGenerator(result.DataList.Select(i => i.PossibleValues.Cast<object>()));
            // todo optymalizacja
         return sequence.Select((data, n) => rewritedItemGenerator(n, new ValueGetter(data.ToArray())));
      }

      private struct ValueGetter : IValueGetter
      {
         private readonly object[] _data;

         public ValueGetter(object[] data)
         {
            _data = data;
         }

         public T GetValue<T>(int key)
         {
            return (T)_data[key];
         }
      }

      /// <summary>
      /// Generate all pairs of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> AllPairs<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int n = 1)
      {
         for (int i = 0; i < n; i++)
            foreach (TItem row in AllPairsInternal(itemGeneratorExpression, (int)DateTime.UtcNow.Ticks, 2))
               yield return row;
      }

      /// <summary>
      /// Generate all pairs of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> AllTuple<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int order, int n = 1)
      {
         for (int i = 0; i < n; i++)
            foreach (TItem row in AllPairsInternal(itemGeneratorExpression, (int)DateTime.UtcNow.Ticks, order))
               yield return row;
      }

      private static IEnumerable<TItem> AllPairsInternal<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int seed, int order)
      {
         if (itemGeneratorExpression == null)
            throw new ArgumentNullException("itemGeneratorExpression");

         GeneratorAnalizerHelper.ValidateItemGeneratorExpression(itemGeneratorExpression.Body);

         ItemGeneratorRewriteResult<TItem, ValueGetter> result =
            GeneratorRewriterHelper.AnalyzeAndRewriteItemGenerator<TItem, ValueGetter>(itemGeneratorExpression, (key, expr) => "");
         Func<int, ValueGetter, TItem> rewritedItemGenerator = result.RewritedItemGeneratorExpression.Compile();

         IEnumerable<IEnumerable<object>> sequence = Utils.AllPairs(result.DataList, seed, order);
         return sequence.Select((data, n) => rewritedItemGenerator(n, new ValueGetter(data.ToArray())));
      }
      /*
      public static IEnumerable<TItem> AllPairs2<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         if (itemGeneratorExpression == null)
            throw new ArgumentNullException("itemGeneratorExpression");

         Expression body = itemGeneratorExpression.Body;
         GeneratorAnalizerHelper.ValidateItemGeneratorExpression(body);

         Dictionary<MethodCallExpression, string> dict = GeneratorAnalizerHelper.FindAllProperUsePlaceHolderMethod(body);

         Func<int, MethodCallExpression, string> parameterNameGenerator =
            (key, methodCallExpression) => dict[methodCallExpression];
         ItemGeneratorRewriteResult<TItem, ValueGetter> result =
            GeneratorRewriterHelper.AnalyzeAndRewriteItemGenerator<TItem, ValueGetter>(itemGeneratorExpression, parameterNameGenerator);

         Func<int, ValueGetter, TItem> rewritedItemGenerator = result.RewritedItemGeneratorExpression.Compile();

         IEnumerable<IEnumerable<object>> cartesianProduct = Utils.CartesianProduct(result.DataList.Select(i => i.PossibleValues.Cast<object>()));

         return cartesianProduct.Select((data, n) => rewritedItemGenerator(n, new ValueGetter(data.ToArray())));
      }*/
   }
}
