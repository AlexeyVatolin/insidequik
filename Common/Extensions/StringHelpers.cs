using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StringHelpers
    {
        public static bool EqualsIgnoreCase(this string self, string value)
        {
            return self.Equals(value, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
