using Microsoft.Extensions.Options;
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

        private readonly ILoggingFacility<TaxCalculator> Log;

        /// <summary>
        /// Initializes the <see cref="TaxCalculator" />
        /// </summary>
        /// <param name="log"></param>
        /// <param name="options">The configuration of the tax system.</param>
        public TaxCalculator(ILoggingFacility<TaxCalculator> log, IOptions<TaxSettings> options)
        {
            var settings = options.Value;

            if (settings.TaxTreshold < 0)
            {
                throw new NotSupportedException($"{nameof(settings.TaxTreshold)} cannot be a negative number.");
            }
            else if (settings.TaxTreshold > settings.SocialContributionCeiling)
            {
                throw new NotSupportedException($"The {nameof(settings.SocialContributionCeiling)} cannot be lower than the {nameof(settings.TaxTreshold)}.");
            }
            else if (settings.TaxSize < 0 || settings.SocialContributionSize < 0)
            {
                throw new NotSupportedException("There is no such a government :(");
            }

            TaxFactor = Convert.ToDecimal(settings.TaxSize / 100);
            TaxFreeCeiling = settings.TaxTreshold;

            SocialInsuranceCeiling = settings.SocialContributionCeiling - settings.TaxTreshold;
            SocialInsuranceFactor = Convert.ToDecimal(settings.SocialContributionSize / 100);

            Log = log;

            Log.Debug($"{nameof(TaxCalculator)} initialized with the follwing {nameof(TaxSettings)}: {settings}");
        }

        /// <summary>
        /// Calculates the net amount by deducting the taxes from the specified <paramref name="grossAmount"/>
        /// </summary>
        /// <param name="grossAmount">The amount before taxes.</param>
        /// <returns></returns>
        public decimal Trim(decimal grossAmount)
        {
            var result = grossAmount < 0 ? 0 : grossAmount;

            if (result == 0)
            {
                Log.Debug($"A negative gross amount ({nameof(grossAmount)}) is being ignored.");
            }
            else
            {
                if (grossAmount >= TaxFreeCeiling)
                {
                    Log.Trace($"Deducting tax from {grossAmount} gross amount...");

                    var taxableAmount = result - TaxFreeCeiling;

                    result -= taxableAmount * TaxFactor;

                    var socialInsuranceAmount = result - TaxFreeCeiling;

                    if (socialInsuranceAmount > SocialInsuranceCeiling)
                    {
                        socialInsuranceAmount = SocialInsuranceCeiling;
                    }

                    Log.Trace($"Deducting social contributions from {socialInsuranceAmount} taxable amount...");

                    result -= socialInsuranceAmount * SocialInsuranceFactor;
                }
                else
                {
                    Log.Debug($"The gross amount is too small for any taxation: {grossAmount}");
                }
            }

            return Math.Round(result, 2);
        }
    }
}
