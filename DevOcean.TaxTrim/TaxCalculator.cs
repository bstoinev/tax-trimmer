using System;

namespace DevOcean.TaxTrim
{
    /// <summary>
    /// Provides facility to calculate a net amount by trimming taxes off of a gross amount.
    /// </summary>
    public class TaxCalculator
    {
        private decimal TaxFreeCeiling = 1000;
        private decimal TaxFactor = 0.1m;
        private decimal SocialInsuranceFactor = 0.15m;
        private decimal SocialInsuranceCeiling = 2000;

        /// <summary>
        /// Initializes the <see cref="TaxCalculator" />
        /// </summary>
        /// <param name="taxFreeCeiling">The maximum amount that is free of taxes.</param>
        /// <param name="taxSize">The size, in percent of the tax.</param>
        /// <param name="socialInsuranceCeiling">The maximum amount from which social contributions are deducted.</param>
        /// <param name="socialInsuranceSize">The size, in percent of the social contributions tax.</param>
        public TaxCalculator(decimal taxFreeCeiling, float taxSize, decimal socialInsuranceCeiling, float socialInsuranceSize)
        {
            if (taxFreeCeiling < 0)
            {
                throw new NotSupportedException($"{nameof(taxFreeCeiling)} cannot be a negative number.");
            }
            else if (taxFreeCeiling > socialInsuranceCeiling)
            {
                throw new NotSupportedException($"The {nameof(socialInsuranceCeiling)} cannot be lower than the {nameof(taxFreeCeiling)}.");
            }
            else if (taxSize < 0 || socialInsuranceSize < 0)
            {
                throw new NotSupportedException("There is no such a government :(");
            }

            TaxFactor = Convert.ToDecimal(taxSize / 100);
            TaxFreeCeiling = taxFreeCeiling;

            SocialInsuranceCeiling = socialInsuranceCeiling - taxFreeCeiling;
            SocialInsuranceFactor = Convert.ToDecimal(socialInsuranceSize / 100);
        }

        /// <summary>
        /// Calculates the net amount by deducting the taxes from the specified <paramref name="grossAmount"/>
        /// </summary>
        /// <param name="grossAmount">The amount before taxes.</param>
        /// <returns></returns>
        public decimal Trim(decimal grossAmount)
        {
            var result = grossAmount < 0 ? 0 : grossAmount;

            if (grossAmount >= TaxFreeCeiling)
            {
                var taxableAmount = result - TaxFreeCeiling;

                result -= taxableAmount * TaxFactor;

                var socialInsuranceAmount = result - TaxFreeCeiling;

                if (socialInsuranceAmount > SocialInsuranceCeiling)
                {
                    socialInsuranceAmount = SocialInsuranceCeiling;
                }

                result -= socialInsuranceAmount * SocialInsuranceFactor;
            }

            return Math.Round(result, 2);
        }
    }
}
