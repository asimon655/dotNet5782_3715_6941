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
                IEnumerable<String>  propertyStrings = from prop in GetType().GetProperties()
                                      select $"{prop.Name} : {(prop.GetValue(this) is null  ?   " None" : prop.GetValue(this).ToString() ) }";
                return string.Join("\n", propertyStrings);
            }
        }
    }
}