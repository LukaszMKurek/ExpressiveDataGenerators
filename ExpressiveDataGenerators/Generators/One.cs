using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// Specifi possible values that can be returned.
   /// </summary>
   public static class One
   {
      /// <summary>
      /// Function would not be called. They only specifi a possible values that can be inserted instead function call.
      /// </summary>
      public static T Of<T>(params T[] items) // todo wersja z nazwą parametru rozwiązywała by problem ze ścieżką parametru
      {
         throw new InvalidOperationException("Function can not be call directly.");
      }
   }
}
