using UnityEngine;

namespace CT.Utils
{
    public class HexColorUtils
    {
        private static int _stringOffset = 0;

        public static Color HexToColor(string hex)
        {
            byte r = byte.MinValue;
            byte g = byte.MinValue;
            byte b = byte.MinValue;
            byte a = byte.MaxValue;

            if (!string.IsNullOrEmpty(hex))
            {
                //Check if HEX was set with a # symbol
                if (hex.StartsWith("#", System.StringComparison.CurrentCulture))
                {
                    _stringOffset++;
                }

                r = byte.Parse(hex.Substring(0 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);

                if (hex.Length > 7)
                {
                    a = byte.Parse(hex.Substring(6 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }

            return new Color32(r, g, b, a);
        }
    }
}