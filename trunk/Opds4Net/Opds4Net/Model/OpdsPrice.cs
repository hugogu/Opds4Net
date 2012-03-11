using System;

namespace Opds4Net.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsPrice
    {
        private string currencyCode = CurrencyCodes.CurrentCurrencyCode;

        /// <summary>
        /// 
        /// </summary>
        public OpdsPrice()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public OpdsPrice(Decimal price)
        {
            Price = price;
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrencyCode
        {
            get { return currencyCode; }
            set
            {
                if (CurrencyCodes.IsValid(value))
                    currencyCode = value;
                else
                    throw new ArgumentOutOfRangeException(String.Format("{0} is not a valid currency code.", value));
            }
        }
    }
}
