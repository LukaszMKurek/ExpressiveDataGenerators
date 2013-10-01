using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ExpressiveDataGenerators;

namespace GeneratorsTests
{
   [TestFixture]
   public sealed class DemoTests
   {
      [Test]
      public void AllCombination()
      {
         var rnd = new Random(0);
         const int l = 3;
         A[] results = Generate.AllCombinations(i => // Generate sequence of object A. Each object was created with different combination of possible values defined by methods One.Of(...)
            new A
            {
               P1 = One.Of(1, 2 * l, 3), // Method define posible values that they can return. One.Of(...) will be newer call. During Expression rewrite it was replaced.
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
         Obj[] seq = Combine.AllCombinations<Obj>(cfg =>    // Generate sequence of object type Obj. Each object will be initialized by unique combination of step alternatives
         {
            cfg.OneOf(o => o.SetA(5), o => o.A = 6);    // One build step can have serveral variants. In this examle parameters "o" are instance of class Obj
            cfg.SetOneOf((o, val) => o.SetB(val), 1, 2, 3);    // One build step can be made with several diferent input data
            cfg.OneOf(
               o => o.C = 4,
               o =>
               {
                  if (o.A == 5 && o.B == 2)
                     cfg.SkipCase();     // Some combinations can be invalid so we can skip it
                  if (o.A == 6 && o.B == 2)
                     o.C = 1;
                  else
                     o.C = 7;
               });
         }).ToArray(); // Return lazy IEnumerable<Obj>

         string seqStr = seq.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();

         Console.WriteLine(seqStr);
      }

      [Test]
      public void Combinators_alternative_usage()
      {
         Obj[] seq = Combine.AllCombinations(() => new Obj(), cfg => // First delegate deliver object that will be subject of step alternatives
         {
            cfg.OneOf(o => o.SetA(5), o => o.A = 6);
            cfg.SetOneOf((o, val) => o.SetB(val), 1, 2).OrOneOf((o, val) => o.SetB(val), 3);
            cfg.OneOf(
               o => o.C = 4,
               o =>
               {
                  if (o.A == 5 && o.B == 2)
                     cfg.SkipCase();
                  if (o.A == 6 && o.B == 2)
                     o.C = 1;
                  else
                     o.C = 7;
               });
         }).ToArray();

         string seqStr = seq.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();

         Console.WriteLine(seqStr);
      }
   }
}