using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    {
        public class Printable
        {
            public override string ToString()
            {
                var propertyStrings = from prop in GetType().GetProperties()
                                      select $"{prop.Name}={prop.GetValue(this)}";
                return string.Join(", ", propertyStrings);
            }
        }
    }
}