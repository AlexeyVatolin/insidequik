using System;
using System.Configuration;

namespace Common.Extensions
{
    public static class StringHelpers
    {
        public static bool EqualsIgnoreCase(this string self, string value)
        {
            return self.Equals(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetConfigurationSetting(this string self)
        {
            return ConfigurationManager.AppSettings[self];
        }

    }
}
