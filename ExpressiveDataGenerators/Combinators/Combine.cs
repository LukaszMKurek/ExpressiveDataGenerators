using System;
using System.Collections.Generic;
using System.Linq;
using ExpressiveDataGenerators.Utils;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// When we have several step to execute with serveral variations each, class can execute for example each combinaion or each pairs of step variations.
   /// </summary>
   public static class Combine
   {
      /// <summary>
      /// Execute each step combination.
      /// </summary>
      /// <param name="configurator"></param>
      public static IEnumerable<T> AllCombinations<T>(Action<GenerateSetup<T>> configurator)
         where T : new()
      {
         return AllCombinations(() => new T(), configurator);
      }

      /// <summary>
      /// Execute each step combination.
      /// </summary>
      /// <param name="creator">Create subject</param>
      /// <param name="configurator"></param>
      public static IEnumerable<T> AllCombinations<T>(Func<T> creator, Action<GenerateSetup<T>> configurator)
      {
         if (creator == null)
            throw new ArgumentNullException("creator");
         if (configurator == null)
            throw new ArgumentNullException("configurator");

         return GenerateSequence(creator, configurator, CombinationStrategies.CartesianProduct);
      }

      /// <summary>
      /// Execute random step combination.
      /// </summary>
      /// <param name="configurator"></param>
      /// <param name="seed"></param>
      public static IEnumerable<T> Random<T>(Action<GenerateSetup<T>> configurator, int? seed = null)
         where T : new()
      {
          return Random(() => new T(), configurator, seed);
      }

      /// <summary>
      /// Execute random step combination.
      /// </summary>
      /// <param name="creator">Create subject</param>
      /// <param name="configurator"></param>
      /// <param name="seed"></param>
      public static IEnumerable<T> Random<T>(Func<T> creator, Action<GenerateSetup<T>> configurator, int? seed = null)
      {
         if (creator == null)
            throw new ArgumentNullException("creator");
         if (configurator == null)
            throw new ArgumentNullException("configurator");

          return GenerateSequence(creator, configurator, i => CombinationStrategies.Random(i, seed));
      }

      /// <summary>
      /// Execute all pairs step combination.
      /// </summary>
      /// <param name="configurator"></param>
      /// <param name="seed"></param>
      /// <param name="sequenceCountMultiplier">How many base AllPair will be called with differents seeds</param>
      public static IEnumerable<T> AllPairs<T>(Action<GenerateSetup<T>> configurator, int? seed = null, int sequenceCountMultiplier = 1)
         where T : new()
      {
         return AllPairs(() => new T(), configurator, seed, sequenceCountMultiplier);
      }

      /// <summary>
      /// Execute all pairs step combination.
      /// </summary>
      /// <param name="creator">Create subject</param>
      /// <param name="configurator"></param>
      /// <param name="seed"></param>
      /// <param name="sequenceCountMultiplier">How many base AllPair will be called with differents seeds</param>
      public static IEnumerable<T> AllPairs<T>(Func<T> creator, Action<GenerateSetup<T>> configurator, int? seed = null, int sequenceCountMultiplier = 1)
      {
         if (creator == null)
            throw new ArgumentNullException("creator");
         if (configurator == null)
            throw new ArgumentNullException("configurator");
         if (sequenceCountMultiplier < 1)
            throw new ArgumentException("sequenceCountMultiplier must be greather than 0.");

         seed = seed ?? (int)DateTime.UtcNow.Ticks;
         for (int i = 0; i < sequenceCountMultiplier; i++)
             foreach (T item in GenerateSequence(creator, configurator, x => CombinationStrategies.AllPairs(x, seed++, 2)))
                 yield return item;
      }

      private static IEnumerable<T> GenerateSequence<T>(
         Func<T> creator, Action<GenerateSetup<T>> configurator,
         Func<IEnumerable<IEnumerable<Action<T>>>, IEnumerable<IEnumerable<Action<T>>>> sequenceGenerator)
      {
         var setup = new GenerateSetup<T>();
         configurator(setup);

         foreach (IEnumerable<Action<T>> setups in sequenceGenerator(setup.SetupVariations))
         {
            T obj = creator();
            setup.IsSkipCase = false;
            foreach (Action<T> action in setups)
            {
               action(obj);
               if (setup.IsSkipCase)
                  break;
            }
            if (setup.IsSkipCase == false)
               yield return obj;
         }
      }
   }
}
