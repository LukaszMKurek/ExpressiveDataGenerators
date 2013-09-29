using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using TestDataGenerators.Combinators;

namespace GeneratorsTests
{
    public class Obj
    {
        public int A;
        public int B;
        public int C;
    }

    [TestFixture]
    public sealed class CombinatorsTests
    {
        [Test]
        public void T01()
        {
            var seq = Combine.AllCases<Obj>(cfg =>
            {
                cfg.OneOf(_ => _.A = 5, _ => _.A = 6);
                cfg.SetOneOf((_, val) => _.B = val, 1, 2, 3);
                cfg.OneOf(
                    _ => _.A = 5,
                    _ =>
                    {
                        if (_.A == 5 && _.B == 2)
                            cfg.SkipCase();
                        _.A = 6;
                    });
            }).ToArray();
        }
    }
}