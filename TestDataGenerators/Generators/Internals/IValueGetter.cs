using System.Linq;
using System.Collections.Generic;
using System;

namespace TestDataGenerators
{
    public interface IValueGetter
    {
        T GetValue<T>(int key);
    }
}