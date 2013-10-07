using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// Generate unique data sequences. Thread safe.
   /// </summary>
   public sealed class SimpleDataSequence
   {
      private long _currNum;
      private long _currStr;
      private long _currGuid;

      /// <summary>
      /// Reset all counters.
      /// </summary>
      public void ResetAll()
      {
         ResetNumericsCounter();
         ResetStringsCounter();
         ResetGuidsCounter();
      }

      /// <summary>
      /// After call this methods Short, Int, Long and Decimal start returning values from 1.
      /// </summary>
      public void ResetNumericsCounter()
      {
         _currNum = 0;
      }

      /// <summary>
      /// After call this methods String and StringDense starts from begining.
      /// </summary>
      public void ResetStringsCounter()
      {
         _currStr = 0;
      }

      /// <summary>
      /// After call this method Guid start from begining.
      /// </summary>
      public void ResetGuidsCounter()
      {
         _currGuid = 0;
      }

      /// <summary>
      /// Generate next Int16 value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      /// <exception cref="InvalidOperationException">When internal counter is greather than Int16.MaxValue</exception>
      public short Short()
      {
         var next = Long();
         if (next > Int16.MaxValue)
            throw new InvalidOperationException("Next Short can not be generated because counter is greather than Int16.MaxValue");
         return (short)next;
      }

      /// <summary>
      /// Generate next Int32 value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      /// <exception cref="InvalidOperationException">When internal counter is greather than Int32.MaxValue</exception>
      public int Int()
      {
         var next = Long();
         if (next > Int32.MaxValue)
            throw new InvalidOperationException("Next Int can not be generated because counter is greather than Int32.MaxValue");
         return (int)next;
      }

      /// <summary>
      /// Generate next Int64 value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      public long Long()
      {
         return Interlocked.Increment(ref _currNum);
      }

      /// <summary>
      /// Generate next Decimal value. Short, Int, Long and Decimal use the same counter.
      /// </summary>
      public decimal Decimal()
      {
         return new decimal(Long());
      }

      /// <summary>
      /// Generate next String value with "_" prefix.
      /// </summary>
      public string String()
      {
         var next = Interlocked.Increment(ref _currStr);
         return "_" + next.ToString(CultureInfo.InvariantCulture);
      }

      /// <summary>
      /// Generate next String value with "_" prefix.
      /// </summary>
      public string String(int maxLength)
      {
         var next = String();
         if (next.Length > maxLength)
            throw new InvalidOperationException("Next String can not be generated because it will be greather than maximum specified length.");
         return next;
      }

      /// <summary>
      /// Generate next String value with using hexidecimal numbers.
      /// </summary>
      public string StringDense()
      {
         var next = Interlocked.Increment(ref _currStr);
         return next.ToString("X");
      }

      /// <summary>
      /// Generate next String value with using hexidecimal numbers.
      /// </summary>
      public string StringDense(int maxLength)
      {
         var next = StringDense();
         if (next.Length > maxLength)
            throw new InvalidOperationException("Next StringDense can not be generated because it will be greather than maximum specified length.");
         return next;
      }

      /// <summary>
      /// Generate next Guid value.
      /// </summary>
      public Guid Guid()
      {
         var next = Interlocked.Increment(ref _currGuid);
         return new Guid((int)(next >> 16), (short)(next & 65535), 0, 0, 0, 0, 0, 0, 0, 0, 0);
      }
   }
}