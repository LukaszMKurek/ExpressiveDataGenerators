using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// Fluent interface to create list of step where each step have several variations.
   /// </summary>
   public sealed class GenerateSetup<TSubject>
   {
      internal readonly List<List<Action<TSubject>>> SetupVariations = new List<List<Action<TSubject>>>();
      internal bool IsSkipCase;

      /// <summary>
      /// Define posible variation of execute one step.
      /// </summary>
      public void OneOf(params Action<TSubject>[] setupVariations)
      {
         if (setupVariations == null)
            throw new ArgumentNullException("setupVariations");
         if (setupVariations.Length == 0)
            throw new ArgumentException("List of setupVariations can not be empty.");

         SetupVariations.Add(setupVariations.ToList());
      }

      /// <summary>
      ///  Define posible variation of execute one step.
      /// </summary>
      /// <param name="setup">Delegate called for each values.</param>
      /// <param name="values">List of values that was potential arguments of setup delegate.</param>
      public Holder<TValue> SetOneOf<TValue>(Action<TSubject, TValue> setup, params TValue[] values)
      {
         if (setup == null)
            throw new ArgumentNullException("setup");
         if (values == null)
            throw new ArgumentNullException("values");
         if (values.Length == 0)
            throw new ArgumentException("List of values can not be empty.");

         var actions = values.Select(val => new Action<TSubject>(subject => setup(subject, val))).ToList();
         SetupVariations.Add(actions);
         return new Holder<TValue>(actions);
      }

      public struct Holder<TValue>
      {
         private readonly List<Action<TSubject>> _actions;

         internal Holder(List<Action<TSubject>> actions)
         {
            _actions = actions;
         }

         /// <summary>
         ///  Define posible variation of execute one step.
         /// </summary>
         /// <param name="setup">Delegate called for each values.</param>
         /// <param name="values">List of values that was potential arguments of setup delegate.</param>
         public Holder<TValue> OrOneOf(Action<TSubject, TValue> setup, params TValue[] values)
         {
            if (setup == null)
               throw new ArgumentNullException("setup");
            if (values == null)
               throw new ArgumentNullException("values");
            if (values.Length == 0)
               throw new ArgumentException("List of values can not be empty.");

            var actions = values.Select(value => new Action<TSubject>(subject => setup(subject, value))).ToList();
            _actions.AddRange(actions);
            return new Holder<TValue>(actions);
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
