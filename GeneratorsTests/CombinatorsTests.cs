using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestDataGenerators.Combinators;

namespace GeneratorsTests
{
   public class Obj
   {
      public int A;
      public int B;
      public int C;

      public void SetA(int a)
      {
         A = a;
      }

      public void SetB(int b)
      {
         B = b;
      }

      public override string ToString()
      {
         return string.Format("[A: {0}, B: {1}, C: {2}]", A, B, C);
      }
   }

   [TestFixture]
   public sealed class CombinatorsTests
   {
      [Test]
      public void AllCases()
      {
         Obj[] seq = Combine.AllCombinations<Obj>(
            cfg =>
            {
               cfg.OneOf(_ => _.A = 5, _ => _.A = 6);
               cfg.SetOneOf((_, val) => _.B = val, 1, 2, 3);
               cfg.OneOf(
                  _ => _.C = 4,
                  _ =>
                  {
                     if (_.A == 5 && _.B == 2)
                        cfg.SkipCase();
                     if (_.A == 6 && _.B == 2)
                        _.C = 1;
                     else
                        _.C = 7;
                  });
            }).ToArray();

         string seqStr = seq.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();

         Console.WriteLine(seqStr);
         Assert.That(seqStr, Is.EqualTo(@"
[A: 5, B: 1, C: 4]
[A: 5, B: 1, C: 7]
[A: 5, B: 2, C: 4]
[A: 5, B: 3, C: 4]
[A: 5, B: 3, C: 7]
[A: 6, B: 1, C: 4]
[A: 6, B: 1, C: 7]
[A: 6, B: 2, C: 4]
[A: 6, B: 2, C: 1]
[A: 6, B: 3, C: 4]
[A: 6, B: 3, C: 7]
".Trim()));
      }

      [Test]
      public void AllPairs()
      {
         Obj[] seq = Combine.AllPairs<Obj>(
            cfg =>
            {
               cfg.OneOf(_ => _.A = 5, _ => _.A = 6);
               cfg.SetOneOf((_, val) => _.B = val, 1, 2, 3);
               cfg.OneOf(
                  _ => _.C = 4,
                  _ =>
                  {
                     if (_.A == 5 && _.B == 2)
                        cfg.SkipCase();
                     if (_.A == 6 && _.B == 2)
                        _.C = 1;
                     else
                        _.C = 7;
                  });
            }).ToArray();

         string seqStr = seq.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();

         Console.WriteLine(seqStr);
         Assert.That(seqStr, Is.EqualTo(@"
[A: 5, B: 1, C: 4]
[A: 5, B: 3, C: 4]
[A: 6, B: 1, C: 7]
[A: 6, B: 2, C: 4]
[A: 6, B: 3, C: 7]
".Trim()));
      }

      [Test]
      public void Random()
      {
         Obj[] seq = Combine.Random<Obj>(
            cfg =>
            {
               cfg.OneOf(_ => _.A = 5, _ => _.A = 6);
               cfg.SetOneOf((_, val) => _.B = val, 1, 2, 3);
               cfg.OneOf(
                  _ => _.C = 4,
                  _ =>
                  {
                     if (_.A == 5 && _.B == 2)
                        cfg.SkipCase();
                     if (_.A == 6 && _.B == 2)
                        _.C = 1;
                     else
                        _.C = 7;
                  });
            }).Take(50).ToArray();

         string seqStr = seq.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(seqStr);

         foreach (var item in seq)
         {
            Assert.That(item.A == 5 || item.A == 6);
            Assert.That(item.B == 1 || item.B == 2 || item.B == 3);
            Assert.That(item.C == 4 || item.C == 7 || item.C == 1);
            if (item.A == 5 && item.B == 2)
               Assert.That(item.C == 4);
            if (item.A == 6 && item.B == 2)
               Assert.That(item.C == 1 || item.C == 4);
         }
      }
   }
}
