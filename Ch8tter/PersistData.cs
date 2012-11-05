using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Ch8tter
{
    class PersistData
    {
        public static bool GetSerializedBoolValue(string name, bool defaultValue = false, bool roaming = true)
        {
            var value = GetSerializedStringValue(name, roaming);
            if (value == null)
                return defaultValue;

            bool result;
            if (bool.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        public static int GetSerializedIntValue(string name, int defaultValue = 0, bool roaming = true)
        {
            var value = GetSerializedStringValue(name, roaming);
            if (value == null)
                return defaultValue;

            int result;
            if (int.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        public static double GetSerializedDoubleValue(string name, double defaultValue = 0, bool roaming = true)
        {
            var value = GetSerializedStringValue(name, roaming);
            if (value == null)
                return defaultValue;

            double result;
            if (double.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out result))
                return result;

            return defaultValue;
        }

        public static string GetSerializedStringValue(string name, bool roaming = true)
        {
            if (roaming && ApplicationData.Current.RoamingSettings.Values.ContainsKey(name))
            {
                return ApplicationData.Current.RoamingSettings.Values[name].ToString();
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(name))
            {
                return ApplicationData.Current.LocalSettings.Values[name].ToString();
            }

            return null;
        }

        public static void SetSerializedValue(string name, object value, bool roaming = true)
        {
            string valueToStore;

            if (value is double)
                valueToStore = ((double)value).ToString(CultureInfo.InvariantCulture);
            else
                valueToStore = value.ToString();

            if (roaming)
                ApplicationData.Current.RoamingSettings.Values[name] = valueToStore;
            ApplicationData.Current.LocalSettings.Values[name] = valueToStore;
        }
    }
}
