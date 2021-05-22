using DevOcean.TaxTrim;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace Devocean.TaxTrim.Xunit
{
    public class TaxCalulatorUnitTests
    {
        [Theory]
        [InlineData(980, 980)]
        [InlineData(3400, 2860)]
        [InlineData(1111, 1084.92)]
        [InlineData(-110, 0)]
        public void PositiveUseCase(decimal grossAmountInput, decimal expectedNetAmount)
        {
            var settings = new TaxSettings
            {
                SocialContributionCeiling = 3000,
                SocialContributionSize = 15,
                TaxSize = 10,
                TaxTreshold = 1000
            };

            var optionsMoq = new Mock<IOptions<TaxSettings>>();
            optionsMoq.SetupGet(o => o.Value).Returns(settings);

            var log = Mock.Of<ILoggingFacility<TaxCalculator>>();

            var sut = new TaxCalculator(log, optionsMoq.Object);

            var test = sut.Trim(grossAmountInput);

            Assert.Equal(expectedNetAmount, test);
        }

        [Fact]
        public void ConstructionFailsWithInvalidSettings()
        {
            var log = Mock.Of<ILoggingFacility<TaxCalculator>>();

            var settings = new TaxSettings
            {
                SocialContributionCeiling = 3000,
                SocialContributionSize = 15,
                TaxSize = 10,
                TaxTreshold = 1000
            };

            var optionsMoq = new Mock<IOptions<TaxSettings>>();
            optionsMoq.SetupGet(o => o.Value).Returns(settings);

            settings.TaxTreshold = -1;

            var negativeTaxTreshold = Assert.Throws<NotSupportedException>(() => new TaxCalculator(log, optionsMoq.Object));
            Assert.IsAssignableFrom<Exception>(negativeTaxTreshold);

            settings.TaxTreshold = 4000;

            var tooBigTresholdSize = Assert.Throws<NotSupportedException>(() => new TaxCalculator(log, optionsMoq.Object));
            Assert.IsAssignableFrom<Exception>(tooBigTresholdSize);

            // Restore valid TaxTreshold
            settings.TaxTreshold = 1000;
            
            settings.TaxSize = -1;

            var negativeTaxSize = Assert.Throws<NotSupportedException>(() => new TaxCalculator(log, optionsMoq.Object));
            Assert.IsAssignableFrom<Exception>(negativeTaxSize);

            // Restore valid TaxSize
            settings.TaxSize = 10;
            
            settings.SocialContributionSize = -1;

            var negativeContributionSize = Assert.Throws<NotSupportedException>(() => new TaxCalculator(log, optionsMoq.Object));
            Assert.IsAssignableFrom<Exception>(negativeContributionSize);

        }
    }
}
