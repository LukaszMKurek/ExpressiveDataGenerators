using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ExpressiveDataGenerators;

namespace GeneratorsTests
{
   public sealed class A
   {
      public int P1;
      public B P2;

      public override string ToString()
      {
         return string.Format("P1: {0}, P2: {1}", P1, P2);
      }
   }

   public sealed class B
   {
      public int P1;
      public string P2;
      public int P3;

      public override string ToString()
      {
         return string.Format("P1: {0}, P2: {1}, P3: {2}", P1, P2, P3);
      }
   }

   [TestFixture]
   public sealed class GeneratorsTests
   {
      [Test]
      public void AllCombination()
      {
         var rnd = new Random(0);
         int l = 3;
         A[] results = Generate.AllCombinations(
            i =>
               new A
               {
                  P1 = One.Of(1, 2*l, 3),
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
      public void AllCombination_2()
      {
         var rnd = new Random(0);
         int l = 3;
         var results = Generate.AllCombinations(
            i =>
               new
               {
                  p1 = One.Of(1, 2*l, 3),
                  p2 = new
                  {
                     p1 = One.Of(1, 2) == 1 ? 2 : 3,
                     p2 = "3",
                     P3 = rnd.Next(20)
                  }
               }).ToArray();

         Assert.That(results.Length, Is.EqualTo(6));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void AllPairs()
      {
         var rnd = new Random(0);
         int l = 3;
         A[] results = Generate.AllPairs(
            i =>
               new A
               {
                  P1 = One.Of(1, 2*l, 3),
                  P2 = new B
                  {
                     P1 = One.Of(1, 2),
                     P2 = "3",
                     P3 = rnd.Next(20)
                  }
               }).ToArray();

         Assert.That(results.Length, Is.EqualTo(6));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void AllPairs_2()
      {
         var rnd = new Random(0);
         int l = 3;
         var results = Generate.AllPairs(
            i =>
               new
               {
                  p1 = One.Of(1, 2*l, 3),
                  p2 = new
                  {
                     p1 = One.Of(1, 2),
                     p2 = "3",
                     P3 = rnd.Next(20)
                  }
               }).ToArray();

         Assert.That(results.Length, Is.EqualTo(6));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void Random()
      {
         var rnd = new Random(0);
         int l = 3;
         var results = Generate.Random(
            i =>
               new
               {
                  p1 = One.Of(1, 2*l, 3),
                  p2 = new
                  {
                     p1 = One.Of(1, 2),
                     p2 = "3",
                     P3 = rnd.Next(20)
                  }
               }).Take(6).ToArray();

         Assert.That(results.Length, Is.EqualTo(6));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void Sequence()
      {
         var rnd = new Random(0);
         int l = 3;
         var results = Generate.Sequence(
            i =>
               new
               {
                  p1 = One.Of(1, 2*l, 3),
                  p2 = new
                  {
                     p1 = One.Of(1, 2),
                     p2 = "3",
                     P3 = rnd.Next(20)
                  }
               }).ToArray();

         Assert.That(results.Length, Is.EqualTo(3));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void SequenceInfinitive()
      {
         var rnd = new Random(0);
         int l = 3;
         var results = Generate.SequenceInfinitive(
            i =>
               new
               {
                  p1 = One.Of(1, 2*l, 3),
                  p2 = new
                  {
                     p1 = One.Of(1, 2),
                     p2 = "3",
                     P3 = rnd.Next(20)
                  }
               }).Take(18).ToArray();

         Assert.That(results.Length, Is.EqualTo(18));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }

      [Test]
      public void SequenceStrict()
      {
         var rnd = new Random(0);
         int l = 3;
         var results = Generate.SequenceStrict(
            i =>
               new
               {
                  p1 = One.Of(1, 2*l, 3),
                  p2 = new
                  {
                     p1 = One.Of(1, 2, 3),
                     p2 = "3"
                  }
               }).ToArray();

         Assert.That(results.Length, Is.EqualTo(3));

         string str = results.Aggregate("", (current, obj) => current + (obj + "\n")).Trim();
         Console.WriteLine(str);
      }
   }
}
