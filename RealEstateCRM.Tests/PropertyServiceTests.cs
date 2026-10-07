using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RealEstateCRM.Tests
{
    public class PropertyServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddProperty_ShouldAddPropertyToDatabase()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var property = new Property
            {
                Title = "Commercial Office Space",
                Address = "Corniche El Nile, Beni Suef",
                City = "Beni Suef",
                Price = 2500000,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await context.Properties.AddAsync(property);
            await context.SaveChangesAsync();

            // Assert
            var result = await context.Properties.FirstOrDefaultAsync(p => p.Title == "Commercial Office Space");
            result.Should().NotBeNull();
            result!.Price.Should().Be(2500000);
        }

        [Fact]
        public async Task SoftDeleteProperty_ShouldSetIsDeletedToTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var property = new Property
            {
                Title = "Residential Apartment",
                Address = "Downtown",
                City = "Cairo",
                Price = 1200000,
                IsDeleted = false
            };
            await context.Properties.AddAsync(property);
            await context.SaveChangesAsync();

            // Act
            property.IsDeleted = true;
            context.Properties.Update(property);
            await context.SaveChangesAsync();

            // Assert
            var deletedProperty = await context.Properties.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Title == "Residential Apartment");
            deletedProperty.Should().NotBeNull();
            deletedProperty!.IsDeleted.Should().BeTrue();
        }
    }
}