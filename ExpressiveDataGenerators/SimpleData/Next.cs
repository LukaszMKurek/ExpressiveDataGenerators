using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// Generate unique data sequences. Thread safe.
   /// </summary>
   public static class Next
   {
      private static readonly SimpleDataSequence s_sequence = new SimpleDataSequence();

      /// <summary>
      /// Reset all counters.
      /// </summary>
      public static void ResetAll()
      {
         s_sequence.ResetAll();
      }

      /// <summary>
      /// After call this methods Short, Int, Long and Decimal start returning values from 1.
      /// </summary>
      public static void ResetNumericsCounter()
      {
         s_sequence.ResetNumericsCounter();
      }

      /// <summary>
      /// After call this methods String and StringDense starts from begining.
      /// </summary>
      public static void ResetStringsCounter()
      {
         s_sequence.ResetStringsCounter();
      }

      /// <summary>
      /// After call this method Guid start from begining.
      /// </summary>
      public static void ResetGuidsCounter()
      {
         s_sequence.ResetGuidsCounter();
      }

      /// <summary>
      /// Generate next Int16 value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      /// <exception cref="InvalidOperationException">When internal counter is greather than Int16.MaxValue</exception>
      public static short Short()
      {
         return s_sequence.Short();
      }

      /// <summary>
      /// Generate next Int32 value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      /// <exception cref="InvalidOperationException">When internal counter is greather than Int32.MaxValue</exception>
      public static int Int()
      {
         return s_sequence.Int();
      }

      /// <summary>
      /// Generate next Int64 value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      public static long Long()
      {
         return s_sequence.Long();
      }

      /// <summary>
      /// Generate next Decimal value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      public static decimal Decimal()
      {
         return s_sequence.Decimal();
      }

      /// <summary>
      /// Generate next String value with "_" prefix.
      /// </summary>
      public static string String()
      {
         return s_sequence.String();
      }

      /// <summary>
      /// Generate next String value with "_" prefix.
      /// </summary>
      public static string String(int maxLength)
      {
         return s_sequence.String(maxLength);
      }

      /// <summary>
      /// Generate next String value with using hexidecimal numbers.
      /// </summary>
      public static string StringDense()
      {
         return s_sequence.StringDense();
      }

      /// <summary>
      /// Generate next String value with using hexidecimal numbers.
      /// </summary>
      public static string StringDense(int maxLength)
      {
         return s_sequence.StringDense(maxLength);
      }

      /// <summary>
      /// Generate next Guid value.
      /// </summary>
      public static Guid Guid()
      {
         return s_sequence.Guid();
      }
   }
}
