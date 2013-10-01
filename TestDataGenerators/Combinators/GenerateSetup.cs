using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressiveDataGenerators
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
      public Holder<TVal> SetOneOf<TVal>(Action<T, TVal> setup, params TVal[] vals)
      {
         var actions = vals.Select(i => new Action<T>(_ => setup(_, i))).ToList();
         SetupVariations.Add(actions);
         return new Holder<TVal>(actions);
      }

      public struct Holder<TVal>
      {
         private readonly List<Action<T>> _actions;

         public Holder(List<Action<T>> actions)
         {
            _actions = actions;
         }

         /// <summary>
         ///  Define posible variation of execute one step.
         /// </summary>
         /// <param name="setup">Delegate called for each values.</param>
         /// <param name="vals">List of values that was potential arguments of setup delegate.</param>
         public Holder<TVal> OrOneOf(Action<T, TVal> setup, params TVal[] vals)
         {
            var actions = vals.Select(i => new Action<T>(_ => setup(_, i))).ToList();
            _actions.AddRange(actions);
            return new Holder<TVal>(actions);
         }
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
