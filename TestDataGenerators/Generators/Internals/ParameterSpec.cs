using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ExpressiveDataGenerators
{
    internal struct ParameterSpec
    {
        public readonly int Key;
        public readonly string ParameterName;
        public readonly IEnumerable PossibleValues;

        public ParameterSpec(int key, string parameterName, IEnumerable possibleValues)
        {
            Key = key;
            ParameterName = parameterName;
            PossibleValues = possibleValues;
        }
    }
}