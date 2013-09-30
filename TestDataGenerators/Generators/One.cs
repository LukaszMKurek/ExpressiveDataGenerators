using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataGenerators
{
   /// <summary>
   ///    Specifi posible values that can be returned.
   /// </summary>
   public static class One
   {
      /// <summary>
      ///    Specifi posible values that can be returned.
      /// </summary>
      public static T Of<T>(params T[] a) // todo wersja z nazwą parametru rozwiązywała by problem ze ścieżką parametru
      {
         throw new InvalidOperationException("Function can not be call directly.");
      }
   }
}
