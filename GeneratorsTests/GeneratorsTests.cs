using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestDataGenerators;

namespace GeneratorsTests
{
    public sealed class A
    {
        public int P1;
        public B P2;
    }

    public sealed class B
    {
        public int P1;
        public string P2;
    }

    [TestFixture]
    public sealed class GeneratorsTests
    {
        [Test]
        public void T01()
        {
            int l = 3;
            var results = Generate.AllCombination(i => 
                new A
                {
                    P1 = One.Of(1, 2 * l, 3),
                    P2 = new B
                    {
                        P1 = One.Of(1, 2) == 1 ? 2 : 3,
                        P2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(6));
        }

        [Test]
        public void T02()
        {
            int l = 3;
            var results = Generate.AllCombination(i => 
                new
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new
                    {
                        p1 = One.Of(1, 2) == 1 ? 2 : 3,
                        p2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(6));
        }

        [Test]
        public void T01_2()
        {
            int l = 3;
            var results = Generate.AllPairs(i =>
                new A
                {
                    P1 = One.Of(1, 2 * l, 3),
                    P2 = new B
                    {
                        P1 = One.Of(1, 2),
                        P2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(6));
        }

        [Test]
        public void T02_2()
        {
            int l = 3;
            var results = Generate.AllPairs(i =>
                new
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new
                    {
                        p1 = One.Of(1, 2),
                        p2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(6));
        }

        [Test]
        public void T03()
        {
            int l = 3;
            var results = Generate.Random(i =>
                new
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new
                    {
                        p1 = One.Of(1, 2),
                        p2 = "3"
                    }
                }).Take(6).ToArray();

            Assert.That(results.Length, Is.EqualTo(6));
        }

        [Test]
        public void T04()
        {
            int l = 3;
            var results = Generate.Sequence(i =>
                new
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new
                    {
                        p1 = One.Of(1, 2),
                        p2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(3));
        }

        [Test]
        public void T05()
        {
            int l = 3;
            var results = Generate.SequenceInfinitive(i =>
                new
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new
                    {
                        p1 = One.Of(1, 2),
                        p2 = "3"
                    }
                }).Take(18).ToArray();

            Assert.That(results.Length, Is.EqualTo(18));
        }

        [Test]
        public void T06()
        {
            int l = 3;
            var results = Generate.Sequence(i =>
                new
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new
                    {
                        p1 = One.Of(1, 2, 3),
                        p2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(3));
        }
    }
}
