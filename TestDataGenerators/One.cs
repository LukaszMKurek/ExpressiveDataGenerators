using System.Linq;
using System.Collections.Generic;
using System;

namespace TestDataGenerators
{
    /// <summary>
    /// Klasa specyfikująca możliwe wartości jakie może przyjąć w wyniku. 
    /// </summary>
    public static class One
    {
        /// <summary>
        /// Funkcja specyfikująca możłiwe wartości jakie może przyjąć w wyniku. 
        /// </summary>
        public static T Of<T>(params T[] a) // todo wersja z nazwą parametru rozwiązywała by problem ze ścieżką parametru
        {
            throw new InvalidOperationException("Zgodnie z projektem metoda ta nie może być nigdy wywołana.");
        }
    }
}