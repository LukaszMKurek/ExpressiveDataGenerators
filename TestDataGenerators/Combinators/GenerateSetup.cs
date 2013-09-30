using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataGenerators.Combinators
{
   /// <summary>
   /// Fluent interface to create list of step where each step have several variations.
   /// </summary>
   public sealed class GenerateSetup<T>
   {
      internal List<List<Action<T>>> SetupVariations = new List<List<Action<T>>>();
      internal bool IsSkipCase;

      /// <summary>
      /// Define posible variation of execute one step.
      /// </summary>
      public void OneOf(params Action<T>[] setupVariations)
      {
         SetupVariations.Add(setupVariations.ToList());
      }

      /// <summary>
      ///  Define posible variation of execute one step.
      /// </summary>
      /// <param name="setup">Delegate called for each values.</param>
      /// <param name="vals">List of values that was potential arguments of setup delegate.</param>
      public void SetOneOf<TVal>(Action<T, TVal> setup, params TVal[] vals)
      {
         SetupVariations.Add(vals.Select(i => new Action<T>(_ => setup(_, i))).ToList());
      }

      /// <summary>
      /// Skip current combination.
      /// </summary>
      public void SkipCase()
      {
         IsSkipCase = true;
      }
   }
}
