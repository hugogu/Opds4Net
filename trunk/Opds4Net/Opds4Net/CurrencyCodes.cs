using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Opds4Net
{
    /// <summary>
    /// Accroding to http://en.wikipedia.org/wiki/ISO_4217
    /// The complete list availible at http://www.currency-iso.org/dl_iso_table_a1.xml
    /// </summary>
    public static class CurrencyCodes
    {
        private static IEnumerable<string> validCurrencyCodes;

        static CurrencyCodes()
        {
            var specifiedCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            validCurrencyCodes = specifiedCultures.Select(c => new RegionInfo(c.LCID).ISOCurrencySymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetCurrencyCode(CultureInfo culture)
        {
            return new RegionInfo(culture.LCID).ISOCurrencySymbol;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public static string GetCurrencyCode(string cultureName)
        {
            return new RegionInfo(CultureInfo.GetCultureInfo(cultureName).LCID).ISOCurrencySymbol;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string CurrentCurrencyCode
        {
            get { return GetCurrencyCode(CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencySymbol"></param>
        /// <returns></returns>
        public static bool IsValid(string currencySymbol)
        {
            return validCurrencyCodes.Contains(currencySymbol);
        }
    }
}
