using FluentAssertions;
using RealEstateCRM.Core.Entities;
using Xunit;

namespace RealEstateCRM.Tests
{
    public class DealsServiceTests
    {
        [Theory]
        [InlineData(1000000, 2.5, 25000)]
        [InlineData(2000000, 3.0, 60000)]
        public void CalculateCommission_ShouldReturnCorrectAmount(decimal salePrice, decimal commissionRate, decimal expectedCommission)
        {
            // Act
            decimal actualCommission = salePrice * (commissionRate / 100);

            // Assert
            actualCommission.Should().Be(expectedCommission);
        }
    }
}