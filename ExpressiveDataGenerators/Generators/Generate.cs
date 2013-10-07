using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressiveDataGenerators.Utils;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// Generate test case seqences.
   /// </summary>
   public static class Generate // todo jak rozszeżać? partial wypali?
   {
      /// <summary>
      /// Generate cartesian product of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> AllCombinations<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, CombinationStrategies.CartesianProduct);
      }

      /// <summary>
      /// Generate infinitive random tuple of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> Randoms<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int? seed = null)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, i => CombinationStrategies.Random(i, seed));
      }

      /// <summary>
      /// Generate sequential tuple of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> Sequence<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, CombinationStrategies.Sequence);
      }

      /// <summary>
      /// Generate infinitive sequential tuple of values specisied in One.Of(...)
      /// </summary>
      public static IEnumerable<TItem> SequenceInfinitive<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, CombinationStrategies.InfinitiveSequence);
      }

      /// <summary>
      /// Generate sequential tuple of values specisied in One.Of(...). Each One.Of must define equals numbers of elements.
      /// </summary>
      public static IEnumerable<TItem> SequenceStrict<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression)
      {
         return GenerateSequenceFrom(itemGeneratorExpression, CombinationStrategies.StrictSequence);
      }

      private static IEnumerable<TItem> GenerateSequenceFrom<TItem>(
         Expression<Func<int, TItem>> itemGeneratorExpression,
         Func<IEnumerable<IEnumerable<object>>, IEnumerable<IEnumerable<object>>> sequenceGenerator)
      {
         if (itemGeneratorExpression == null)
            throw new ArgumentNullException("itemGeneratorExpression");

         ItemGeneratorRewriteResult<TItem, ValueGetter> result =
            GeneratorRewriterHelper.AnalyzeAndRewriteItemGenerator<TItem, ValueGetter>(itemGeneratorExpression, (key, expr) => "");
         Func<int, ValueGetter, TItem> rewritedItemGenerator = result.RewritedItemGeneratorExpression.Compile();

         IEnumerable<IEnumerable<object>> sequence = sequenceGenerator(result.DataList.Select(i => i.PossibleValues.Cast<object>()));

         int n = 0;
         var valueGetter = new ValueGetter { Data = new object[result.DataList.Count] };
         foreach (var data in sequence)
         {
            int i = 0;
            foreach (var o in data)
               valueGetter.Data[i++] = o;

            yield return rewritedItemGenerator(n++, valueGetter);
         }
         //return sequence.Select((data, n) => rewritedItemGenerator(n, new ValueGetter(data.ToArray())));
      }

      private struct ValueGetter : IValueGetter
      {
         internal object[] Data;

         public T GetValue<T>(int key)
         {
            return (T)Data[key];
         }
      }
      
      /// <summary>
      /// Generate all pairs of values specisied in One.Of(...)
      /// </summary>
      /// <param name="n">How many base AllPair will be called with differents seeds</param>
      public static IEnumerable<TItem> AllPairs<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int? seed = null, int n = 1)
      {
         seed = seed ?? (int)DateTime.UtcNow.Ticks;
         for (int i = 0; i < n; i++) // todo: parsed several time, side effects, 
            foreach (TItem row in AllPairsInternal(itemGeneratorExpression, seed++, 2))
               yield return row;
      }

      /// <summary>
      /// Generate all pairs of values specisied in One.Of(...)
      /// </summary>
      /// <param name="n">How many base AllPair will be called with differents seeds</param>
      public static IEnumerable<TItem> AllTuples<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int order, int? seed = null, int n = 1)
      {
         seed = seed ?? (int)DateTime.UtcNow.Ticks;
         for (int i = 0; i < n; i++)
            foreach (TItem row in AllPairsInternal(itemGeneratorExpression, seed++, order))
               yield return row;
      }

      private static IEnumerable<TItem> AllPairsInternal<TItem>(Expression<Func<int, TItem>> itemGeneratorExpression, int? seed, int order)
      {
         if (itemGeneratorExpression == null)
            throw new ArgumentNullException("itemGeneratorExpression");

         ItemGeneratorRewriteResult<TItem, ValueGetter> result =
            GeneratorRewriterHelper.AnalyzeAndRewriteItemGenerator<TItem, ValueGetter>(itemGeneratorExpression, (key, expr) => "");
         Func<int, ValueGetter, TItem> rewritedItemGenerator = result.RewritedItemGeneratorExpression.Compile();

         IEnumerable<IEnumerable<object>> sequence = CombinationStrategies.AllPairs(result.DataList, seed, order);

         int n = 0;
         var valueGetter = new ValueGetter { Data = new object[result.DataList.Count] };
         foreach (var data in sequence)
         {
            int i = 0;
            foreach (var o in data)
               valueGetter.Data[i++] = o;

            yield return rewritedItemGenerator(n++, valueGetter);
         }
         //return sequence.Select((data, n) => rewritedItemGenerator(n, new ValueGetter(data.ToArray())));
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
