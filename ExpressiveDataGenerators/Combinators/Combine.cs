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
      public static IEnumerable<T> AllCombinations<T>(Action<GenerateSetup<T>> cfg)
         where T : new()
      {
         return AllCombinations(() => new T(), cfg);
      }

      /// <summary>
      /// Execute each step combination.
      /// </summary>
      public static IEnumerable<T> AllCombinations<T>(Func<T> creator, Action<GenerateSetup<T>> cfg)
      {
         return GenerateSequence(creator, cfg, CombinationStrategies.CartesianProduct);
      }

      /// <summary>
      /// Execute random step combination.
      /// </summary>
      public static IEnumerable<T> Random<T>(Action<GenerateSetup<T>> cfg)
         where T : new()
      {
         return Random(() => new T(), cfg);
      }

      /// <summary>
      /// Execute random step combination.
      /// </summary>
      public static IEnumerable<T> Random<T>(Func<T> creator, Action<GenerateSetup<T>> cfg)
      {
         return GenerateSequence(creator, cfg, CombinationStrategies.Random);
      }

      /// <summary>
      /// Execute all pair step combination.
      /// </summary>
      public static IEnumerable<T> AllPairs<T>(Action<GenerateSetup<T>> cfg)
         where T : new()
      {
         return AllPairs(() => new T(), cfg);
      }

      /// <summary>
      /// Execute all pair step combination.
      /// </summary>
      public static IEnumerable<T> AllPairs<T>(Func<T> creator, Action<GenerateSetup<T>> cfg)
      {
         return GenerateSequence(creator, cfg, i => CombinationStrategies.AllPairs(i, (int)DateTime.UtcNow.Ticks, 2));
      }

      private static IEnumerable<T> GenerateSequence<T>(
         Func<T> creator, Action<GenerateSetup<T>> cfg,
         Func<IEnumerable<IEnumerable<Action<T>>>, IEnumerable<IEnumerable<Action<T>>>> sequenceGenerator)
      {
         var setup = new GenerateSetup<T>();
         cfg(setup);

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
