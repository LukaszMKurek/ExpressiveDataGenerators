using System.Linq;
using System.Collections.Generic;
using System;

namespace ExpressiveDataGenerators
{
    internal interface IValueGetter
    {
        T GetValue<T>(int key);
    }
}