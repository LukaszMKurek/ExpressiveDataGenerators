using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ExpressiveDataGenerators
{
   /// <summary>
   /// Simple random data generator.
   /// </summary>
   public static class Rand
   {
      public const string LETTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
      public const string LETTERS_EXT = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZąĄćĆęĘłŁńŃóÓśŚżŻźŹżŻ";
      public const string DIGITS = "0123456789";
      public const string SPECIAL = "`~!@#$%^&*()-+*/.,><;:?[]{}|\\'\"";
      public const string ALPHA_NUMERICS = DIGITS + LETTERS + "_";
      public const string ALPHA_NUMERICS_EXT = DIGITS + LETTERS_EXT + "_";
      private static Random s_random = new Random();

      /// <summary>
      /// Set seed to internal random generator.
      /// </summary>
      public static void SetSeed(int seed)
      {
         s_random = new Random(seed);
      }

      /// <summary>
      /// Set seed from DateTime.UtcNow to internal random generator.
      /// </summary>
      public static void SetRandomSeed()
      {
         s_random = new Random((int)System.DateTime.UtcNow.Ticks);
      }

      /// <summary>
      /// Random short from Int16.MinValue to Int16.MaxValue. Generate numbers with same probability for each order of magnitude.
      /// </summary>
      public static short Short()
      {
         return (short)((s_random.Next(0, 2) == 1 ? -1 : 1) * (short)(Int32.Parse(String(1, 6, DIGITS), NumberStyles.None) % Int16.MaxValue));
      }

      /// <summary>
      /// Random short from Int13.MinValue to Int16.MaxValue. Generate numbers with same probability for all range.
      /// </summary>
      public static short ShortUniform()
      {
         return Short(Int16.MinValue, Int16.MaxValue);
      }

      /// <summary>
      /// Random short in range &lt;min, max). Generate numbers with same probability for all range.
      /// </summary>
      public static short Short(short min, short max)
      {
         return (short)s_random.Next(min, max);
      }

      /// <summary>
      /// Random int between Int32.MinValue and Int32.MaxValue. Generate numbers with same probability for each order of magnitude.
      /// </summary>
      public static int Int()
      {
         return (s_random.Next(0, 2) == 1 ? -1 : 1) * (int)(Int64.Parse(String(1, 11, DIGITS), NumberStyles.None) % Int32.MaxValue);
      }

      /// <summary>
      /// Random int between Int32.MinValue and Int32.MaxValue. Generate numbers with same probability for all range.
      /// </summary>
      public static int IntUniform()
      {
         return Int(Int32.MinValue, Int32.MaxValue);
      }

      /// <summary>
      /// Random int in range &lt;min, max). Generate numbers with same probability for all range.
      /// </summary>
      public static int Int(int min, int max)
      {
         return s_random.Next(min, max);
      }

      /// <summary>
      /// Random long between Int64.MinValue and Int64.MaxValue. Generate numbers with same probability for each order of magnitude.
      /// </summary>
      public static long Long()
      {
         return (s_random.Next(0, 2) == 1 ? -1L : 1L) * (long)(decimal.Parse(String(1, 22, DIGITS), NumberStyles.None) % Int64.MaxValue);
      }

      /// <summary>
      /// Random long between Int64.MinValue and Int64.MaxValue. Generate numbers with same probability for all range.
      /// </summary>
      public static long LongUniform()
      {
         var buffer = new byte[sizeof(Int64)];
         s_random.NextBytes(buffer);
         return BitConverter.ToInt64(buffer, 0);
      }

      /// <summary>
      /// Random long in range &lt;min, max). Generate numbers with same probability for all range.
      /// </summary>
      public static long Long(long min, long max)
      {
         return min + Math.Abs(Long()) % (max - min);
      }

      /// <summary>
      /// Random double between double.MinValue and double.MaxValue. Generate numbers with same probability for each order of magnitude.
      /// </summary>
      public static double Double()
      {
         return (s_random.Next(0, 2) == 1 ? -1d : 1d) * double.Parse(String(1, 16, DIGITS) + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + String(1, 16, DIGITS), NumberStyles.AllowDecimalPoint);
      }

      /// <summary>
      /// Random double between double.MinValue and double.MaxValue. Generate numbers with same probability for all range.
      /// </summary>
      public static double DoubleUniform()
      {
         return (s_random.NextDouble() - s_random.NextDouble()) * (double.MaxValue * 0.5);
      }

      /// <summary>
      /// Random double in range &lt;min, max). Generate numbers with same probability for all range.
      /// </summary>
      public static double Double(double min, double max)
      {
         return min + s_random.NextDouble() * (max - min);
      }

      /// <summary>
      /// Random decimal between decimal.MinValue and decimal.MinValue. Generate numbers with same probability for each order of magnitude.
      /// </summary>
      public static decimal Decimal()
      {
         var s = (byte)s_random.Next(0, 28);
         var a = Int();
         var b = Int();
         var c = Int();
         var n = s_random.NextDouble() >= 0.5;
         return new Decimal(a, b, c, n, s);
      }

      private static byte GetDecimalScale()
      {
         for (byte i = 0; i <= 28; i++)
            if (s_random.NextDouble() >= 0.1)
               return i;

         return 0;
      }

      /// <summary>
      /// Random decimal between decimal.MinValue and decimal.MinValue. Generate numbers with same probability for all range.
      /// </summary>
      public static decimal DecimalUniform()
      {
         var s = GetDecimalScale();
         var a = Int();
         var b = Int();
         var c = Int();
         var n = s_random.NextDouble() >= 0.5;
         return new Decimal(a, b, c, n, s);
      }

      /// <summary>
      /// Random decimal in range &lt;min, max). Generate numbers with same probability for all range.
      /// </summary>
      public static decimal Decimal(decimal min, decimal max)
      {
         return new decimal(Double((double)min, (double)max));
      }

      /// <summary>
      /// Generate decimal with specified number digits.
      /// </summary>
      public static decimal DecimalWithPrecise(int numberBeforeDot, int numberAfterDot)
      {
         var randNumber = StringLen(30, DIGITS);
         var sign = s_random.Next(0, 2) == 1 ? "-" : "";
         return decimal.Parse(sign + randNumber.Substring(0, numberBeforeDot) + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + randNumber.Substring(numberBeforeDot + 1, numberAfterDot), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
      }

      /// <summary>
      /// Generate random string with specified max length and character set.
      /// </summary>
      public static string String(int maxLength = 256, string chars = ALPHA_NUMERICS)
      {
         int length = Int(0, maxLength);
         return StringLen(length, chars);
      }

      /// <summary>
      /// Generate random string with specified range of length and character set.
      /// </summary>
      public static string String(int minLength, int maxLength, string chars = ALPHA_NUMERICS)
      {
         int length = Int(minLength, maxLength);
         return StringLen(length, chars);
      }

      /// <summary>
      /// Generate random string with specified length and character set.
      /// </summary>
      public static string StringLen(int length, string chars = ALPHA_NUMERICS)
      {
         var builder = new StringBuilder(length);

         for (int i = 0; i < length; ++i)
            builder.Append(chars[s_random.Next(chars.Length)]);

         return builder.ToString();
      }

      /// <summary>
      /// Generate random Guid.
      /// </summary>
      public static Guid Guid()
      {
         return System.Guid.NewGuid();
      }

      /// <summary>
      /// Generated random array of bytes with random length between 0 and 256.
      /// </summary>
      public static byte[] Bytes(int maxLength = 256)
      {
         int length = Int(0, maxLength);
         return BytesLen(length);
      }

      /// <summary>
      /// Generated random array of bytes with specified length.
      /// </summary>
      public static byte[] Bytes(int minLength, int maxLength)
      {
         int length = Int(minLength, maxLength);
         return BytesLen(length);
      }

      /// <summary>
      /// Generated random array of bytes with specified length.
      /// </summary>
      public static byte[] BytesLen(int length)
      {
         var bytes = new byte[length];
         s_random.NextBytes(bytes);

         return bytes;
      }

      /// <summary>
      /// Generate random DateTime. Between 1899.12.27 and 2121.10.23.
      /// </summary>
      public static DateTime DateTime()
      {
         return new DateTime(1899, 12, 27).AddMilliseconds(s_random.NextDouble() * 7000000000000d);
      }

      /// <summary>
      /// Generate random Date. Between 1899.12.27 and 2121.10.23.
      /// </summary>
      public static DateTime Date()
      {
         var dt = DateTime();
         return new DateTime(dt.Year, dt.Month, dt.Day);
      }

      /// <summary>
      /// Generate random DateTime in range &lt;min, max)
      /// </summary>
      public static DateTime DateTime(DateTime start, DateTime end)
      {
         double range = (end - start).TotalMilliseconds;
         return start.AddMilliseconds(s_random.NextDouble() * range);
      }

      /// <summary>
      /// Generate random Date in range &lt;min, max)
      /// </summary>
      public static DateTime Date(DateTime start, DateTime end)
      {
         var dt = DateTime(start, end);
         return new DateTime(dt.Year, dt.Month, dt.Day);
      }

      /// <summary>
      /// Generate random 24h timespan.
      /// </summary>
      public static TimeSpan TimeSpan()
      {
         return new TimeSpan(0, s_random.Next(24), s_random.Next(60), s_random.Next(60), s_random.Next(1000));
      }

      /// <summary>
      /// Get random item from list.
      /// </summary>
      public static T Item<T>(IList<T> list)
      {
         if (list == null)
            throw new ArgumentNullException("list");
         if (list.Count == 0)
            throw new ArgumentException("List must contains any elements.");

         return list[s_random.Next(list.Count)];
      }

      /// <summary>
      /// Get random items from list. Return infinitive sequence.
      /// </summary>
      public static IEnumerable<T> Items<T>(IList<T> list)
      {
         while(true)
            yield return Item(list);
      }

      /// <summary>
      /// Get random item from list or generate from action. Probalility of call action is same like get one concrete element from list, probability can be increase by generatorProbabilityFactor.
      /// </summary>
      public static T Item<T>(IList<T> list, Func<T> valueGenerator, int generatorProbabilityFactor = 1)
      {
         if (list == null)
            throw new ArgumentNullException("list");
         if (valueGenerator == null)
            throw new ArgumentNullException("valueGenerator");
         if (generatorProbabilityFactor < 0)
            throw new ArgumentException("generatorProbabilityFactor must be greather than 0.");
         if (list.Count == 0)
            throw new ArgumentException("List must contains any elements.");
         
         var index = s_random.Next(list.Count + generatorProbabilityFactor);
         if (index >= list.Count)
            return valueGenerator();

         return list[index];
      }

      /// <summary>
      /// Get random items from list or generate from action. Probalility of call action is same like get one concrete element from list, probability can be increase by generatorProbabilityFactor.
      /// </summary>
      public static IEnumerable<T> Items<T>(IList<T> list, Func<T> valueGenerator, int generatorProbabilityFactor = 1)
      {
         while (true)
            yield return Item(list, valueGenerator, generatorProbabilityFactor);
      }

      /// <summary>
      /// Get random item from list.
      /// </summary>
      public static T Item<T>(params T[] items)
      {
         if (items == null)
            throw new ArgumentNullException("items");
         if (items.Length == 0)
            throw new ArgumentException("List must contains any elements.");

         return items[s_random.Next(items.Length)];
      }

      /// <summary>
      /// Get random items from list. Return infinitive sequence.
      /// </summary>
      public static IEnumerable<T> Items<T>(params T[] list)
      {
         while (true)
            yield return Item(list);
      }
   }
}