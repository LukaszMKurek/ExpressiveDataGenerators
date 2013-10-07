using System;
using System.Collections.Generic;
using System.Linq;
using ExpressiveDataGenerators;
using NUnit.Framework;

namespace GeneratorsTests
{
   [TestFixture]
   public sealed class SimpleData
   {
      [Test]
      public void T01()
      {
         var addMilliseconds = new DateTime(1899, 12, 27).AddMilliseconds(7000000000000d);
      }
   }
}
