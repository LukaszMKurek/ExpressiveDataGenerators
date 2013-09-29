using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataGenerators.Combinators
{
    public static class Combine
    {
        public static IEnumerable<T> AllCases<T>(Action<GenerateSetup<T>> cfg)
            where T : new()
        {
            return AllCases(() => new T(), cfg);
        }

        public static IEnumerable<T> AllCases<T>(Func<T> creator, Action<GenerateSetup<T>> cfg)
        {
            var setup = new GenerateSetup<T>();
            cfg(setup);

            foreach (var setups in Utils.CartesianProduct(setup.SetupVariations))
            {
                var obj = creator();
                setup.IsSkipCase = false;
                foreach (var action in setups)
                {
                    action(obj);
                    if (setup.IsSkipCase)
                        goto NextTestCase;
                }
                yield return obj;
            NextTestCase: ;
            }
        }
    }
}
