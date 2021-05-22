namespace DevOcean.TaxTrim
{
    /// <summary>
    /// Provides settings for the taxation system.
    /// </summary>
    public class TaxSettings
    {
        /// <summary>
        /// The maximum amount from which social contributions are deducted.
        /// </summary>
        public decimal SocialContributionCeiling { get; set; }

        /// <summary>
        /// The size, in percent of the social contributions tax.
        /// </summary>
        public float SocialContributionSize { get; set; }

        /// <summary>
        /// The size of the tax, as a percentage.
        /// </summary>
        public float TaxSize { get; set; }

        /// <summary>
        /// The maximum amount that is free of taxes.
        /// </summary>
        public decimal TaxTreshold { get; set; }

        public override string ToString()
        {
            var result = $"{nameof(SocialContributionCeiling)}: {SocialContributionCeiling}; {nameof(SocialContributionSize)}: {SocialContributionSize}; {nameof(TaxSize)}: {TaxSize}; {nameof(TaxTreshold)}: {TaxTreshold}";

            return result;
        }
    }
}
