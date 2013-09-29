using System.Linq;
using System.Collections.Generic;
using System;

namespace TestDataGenerators.Combinators
{
    public sealed class GenerateSetup<T>
    {
        internal List<List<Action<T>>> SetupVariations = new List<List<Action<T>>>();
        internal bool IsSkipCase;

        public void OneOf(params Action<T>[] setupVariations)
        {
            SetupVariations.Add(setupVariations.ToList());
        }

        public void SetOneOf<TVal>(Action<T, TVal> setup, params TVal[] vals)
        {
            SetupVariations.Add(vals.Select(i => new Action<T>(_ => setup(_, i))).ToList());
        }

        public void SkipCase()
        {
            IsSkipCase = true;
        }
    }
}