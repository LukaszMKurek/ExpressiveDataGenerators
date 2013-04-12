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
         var results = Generate.AllCombinations(itemNumber =>
            new
            {
               A = One.Of(1, 2, 3),
               B = One.Of("a", "b", "c"),
               C = One.Of(true, false, (bool?)null)
            });

         Print(results);
      }

      [Test]
      public void AllCombination_2()
      {
         var results = Generate.AllCombinations(itemNumber =>
            SomeExampleFunc( // Suprise: Named parameters are not suppoted in Expressions Tress
               One.Of(1, 2, 3),
               One.Of("a", "b", "c"),
               One.Of(true, false)
            ));

         Print(results);
      }

      private string SomeExampleFunc(int a, string b, bool c)
      {
         return a + b + c;
      }

      [Test]
      public void AllPairs()
      {
         var results = Generate.AllPairs(itemNumber =>
            new
            {
               A = One.Of(1, 2, 3),
               B = One.Of("a", "b", "c"),
               C = One.Of(true, false, (bool?)null)
            }, 0);

         Print(results);
      }

      [Test]
      public void Random100()
      {
         var results = Generate.Randoms(itemNumber =>
            new
            {
               A = One.Of(1, 2, 3),
               B = One.Of("a", "b", "c"),
               C = One.Of(true, false, (bool?)null)
            }, 0).Take(100);

         Print(results);
      }

      [Test]
      public void Sequential()
      {
         var results = Generate.Sequence(itemNumber =>
            new
            {
               A = One.Of(1, 2, 3, 4, 5),
               B = One.Of("a", "b"),
               C = One.Of(true, false)
            }).Take(100);

         Print(results);
      }

      [Test]
      public void SequentialStrict()
      {
         var results = Generate.SequenceStrict(itemNumber =>
            new
            {
               A = One.Of(1, 2, 3),
               B = One.Of("a", "b", "c"),
               C = One.Of(true, false, (bool?)null)
            }).Take(100);

         Print(results);
      }

      [Test]
      public void AllCombination_Alternative_1()
      {
         // First delegate create subject or only inject subject 
         var results = Combine.AllCombinations(() => new Obj(), setup =>
         {
            setup.OneOf( // execute one alternative from set
               subject => subject.SetA(1), // first alternative
               subject => subject.SetA(2)); // second alternative
            setup.OneOf(
               subject => subject.B = 4,
               subject => subject.B = 5);
         });

         Print(results);
      }

      [Test]
      public void AllCombination_Alternative_2()
      {
         var results = Combine.AllCombinations(() => new Obj(), setup =>
         {
            setup.SetOneOf((subject, value) => subject.SetA(value), 1, 2);
            setup.SetOneOf((subject, value) => subject.B = value, 4, 5);
         });

         Print(results);
      }

      [Test]
      public void AllCombination_MoreComplicated()
      {
         var rnd = new Random(0);
         const int l = 3;
         var results = Generate.AllCombinations(i => 
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

         Print(results);
      }

      [Test]
      public void AllCombination_Alternative_WithSkipInvalidCombination()
      {
         var results = Combine.AllCombinations(() => new Obj(), setup =>
         {
            setup.SetOneOf((subject, value) => subject.SetA(value), 1, 2);
            setup.SetOneOf((subject, value) => subject.B = value, 4, 5);
            setup.OneOf(subject =>
            {
               if (subject.A == 1 && subject.B == 4)
                  setup.SkipCase();

               subject.C = subject.A + subject.B;
            });
         });

         Print(results);
      }

      [Test]
      public void Combinators()
      {
         var results = Combine.AllCombinations<Obj>(cfg => 
         {
            cfg.OneOf(o => o.SetA(5), o => o.A = 6); 
            cfg.SetOneOf((o, val) => o.SetB(val), 1, 2, 3); 
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
         });

         Print(results);
      }

      [Test]
      public void Combinators_alternative_usage()
      {
         var results = Combine.AllCombinations(() => new Obj(), cfg => // First delegate deliver object that will be subject of step alternatives
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
         });

         Print(results);
      }

      private static void Print<T>(IEnumerable<T> results)
      {
         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }
   }
}