using DevOcean.TaxTrim;
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
        public void Positive(decimal grossAmountInput, decimal expectedNetAmount)
        {
            var sut = new TaxCalculator(1000, 10, 3000, 15);

            var test = sut.Trim(grossAmountInput);

            Assert.Equal(expectedNetAmount, test);
        }

        [Fact]
        public void ConstructionFailsWithInvalidSettings()
        {
            Assert.Throws<NotSupportedException>(() => new TaxCalculator(123, -1, 234, 1));
            Assert.Throws<NotSupportedException>(() => new TaxCalculator(123, 1, 234, -1));
            Assert.Throws<NotSupportedException>(() => new TaxCalculator(123, 1, 23, 1));
        }
    }
}
