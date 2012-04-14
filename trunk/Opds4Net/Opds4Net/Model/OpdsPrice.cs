using System;

namespace Opds4Net.Model
{
    /// <summary>
    /// The price information defined by OPDS for the purchase link.
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
        /// The sales price for the book.
        /// It is a common requirement to show the original price for the paper edition. But OPDS haven't define that yet.
        /// </summary>
        public Decimal Price { get; set; }

        /// <summary>
        /// The currency code of the price. Must be a three-letters string defined in http://en.wikipedia.org/wiki/ISO_4217.
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
