﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Test.VariationGeneration;

namespace ExpressiveDataGenerators.Utils
{
   public static class CombinationStrategies
   {
      public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
      {
         IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
         return sequences.Aggregate(
            emptyProduct,
            (accumulator, sequence) =>
               from accseq in accumulator
               from item in sequence
               select accseq.Concat(new[] { item }));
      }
       
      public static IEnumerable<IEnumerable<T>> Random<T>(this IEnumerable<IEnumerable<T>> sequences, int? seed)
      {
          var rnd = new Random(seed ?? (int)DateTime.UtcNow.Ticks);
          T[][] data = sequences.Select(i => i.ToArray()).ToArray();
          while (true)
              yield return data.Select(i => i[rnd.Next(i.Length)]);
      }

      private sealed class SequenceHelper<T>
      {
         public T[] Array { get; set; }
         public int Index { get; set; }
      }

      public static IEnumerable<IEnumerable<T>> Sequence<T>(this IEnumerable<IEnumerable<T>> sequences)
      {
         SequenceHelper<T>[] data = sequences.Select(i => new SequenceHelper<T> { Array = i.ToArray(), Index = -1 }).ToArray();
         int count = data.Max(i => i.Array.Length);
         for (int n = 0; n < count; n++)
            yield return data.Select(
               i =>
               {
                  if (++i.Index == i.Array.Length)
                     i.Index = 0;
                  return i.Array[i.Index];
               });
      }

      public static IEnumerable<IEnumerable<T>> InfinitiveSequence<T>(this IEnumerable<IEnumerable<T>> sequences)
      {
         SequenceHelper<T>[] data = sequences.Select(i => new SequenceHelper<T> { Array = i.ToArray(), Index = -1 }).ToArray();
         while (true)
            yield return data.Select(i =>
            {
               if (++i.Index == i.Array.Length)
                  i.Index = 0;
               return i.Array[i.Index];
            });
      }

      public static IEnumerable<IEnumerable<T>> StrictSequence<T>(this IEnumerable<IEnumerable<T>> sequences)
      {
         T[][] data = sequences.Select(i => i.ToArray()).ToArray();
         int count = data[0].Length;
         if (data.Any(arr => arr.Length != count))
            throw new InvalidOperationException("Jedna z sekwencji zawiera liczbę elementów inną niż pozostałe.");

         for (int n = 0; n < count; n++)
            yield return data.Select(i => i[n]);
      }

      public static IEnumerable<IEnumerable<T>> AllPairs<T>(this IEnumerable<IEnumerable<T>> sequences, int? seed = null, int order = 2)
      {
         var p = new List<ParameterBase>();
         int n = 0;
         foreach (var sequence in sequences)
         {
            var parameterValues = new Parameter<T>("_" + n++);
            foreach (var possibleValue in sequence)
               parameterValues.Add(possibleValue);

            p.Add(parameterValues);
         }
         var model = new Model(p);

         return model.GenerateVariations(order, seed ?? (int)DateTime.UtcNow.Ticks).Select(variation => p.Select(i => (T)variation[i.Name]));
      }

      internal static IEnumerable<IEnumerable<object>> AllPairs(IEnumerable<ParameterSpec> parametersSpec, int? seed, int order)
      {
         var p = new List<ParameterBase>();
         foreach (ParameterSpec parameterSpec in parametersSpec)
         {
            var parameterValues = new Parameter<object>("_" + parameterSpec.Key);
            foreach (object possibleValue in parameterSpec.PossibleValues)
               parameterValues.Add(possibleValue);

            p.Add(parameterValues);
         }
         var model = new Model(p);

         return model.GenerateVariations(order, seed ?? (int)DateTime.UtcNow.Ticks).Select(variation => p.Select(i => variation[i.Name]));
      }
   }
}
