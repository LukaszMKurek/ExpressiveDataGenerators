using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestDataGenerators;
using TestDataGenerators.Combinators;

namespace GeneratorsTests
{
   [TestFixture]
   public sealed class DemoTests
   {
      [Test]
      public void AllCombination()
      {
         var rnd = new Random(0);
         int l = 3;
         A[] results = Generate.AllCombinations(i =>
            new A
            {
               P1 = One.Of(1, 2 * l, 3),
               P2 = new B
               {
                  P1 = One.Of(1, 2) == 1 ? 2 : 3,
                  P2 = "3",
                  P3 = rnd.Next(20)
               }
            }).ToArray();

         Assert.That(results.Length, Is.EqualTo(6));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void Combinators()
      {
         Obj[] seq = Combine.AllCombinations<Obj>(cfg =>
         {
            cfg.OneOf(i => i.SetA(5), _ => _.A = 6);
            cfg.SetOneOf((i, val) => i.SetB(val), 1, 2, 3);
            cfg.OneOf(
               i => i.C = 4,
               i =>
               {
                  if (i.A == 5 && i.B == 2)
                     cfg.SkipCase();
                  if (i.A == 6 && i.B == 2)
                     i.C = 1;
                  else
                     i.C = 7;
               });
         }).ToArray();

         string seqStr = seq.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();

         Console.WriteLine(seqStr);
      }
   }
}