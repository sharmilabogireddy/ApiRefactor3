using ApiRefactor.Models;
using ApiRefactor.Repositories;
using ApiRefactor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using ApiRefactor.Data.Contexts;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace ApiRefactor.Test
{
    public class WaveRepositoryTests
    {
        private WaveRepositoryContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<WaveRepositoryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new WaveRepositoryContext(options);
        }

        [Fact]
        public async Task SaveAsync_Should_Add_Wave_And_Return_Entity()
        {
            //Arrange
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);
            var wave = new Wave { Name = "Wave A" };

            //Act
            var result = await repo.SaveAsync(wave);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Wave A");
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task SaveAsync_WhenNameIsEmpty_ShouldThrowValidationException()
        {
            //Arrange
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);
            var wave = new Wave { Name = "" };

            // Act
            Func<Task> act = () => repo.SaveAsync(wave);

            //Assert
            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("*name*");
        }

        [Fact]
        public async Task UpdateAsync_WhenWaveIsValid_ShouldUpdateAndReturnWave()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);
            var original = await repo.SaveAsync(new Wave { Name = "Original" });

            // Act
            original.Name = "Updated";
            var result = await repo.UpdateAsync(original);

            // Assert
            result.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task UpdateAsync_WhenIdIsEmpty_ShouldThrowArgumentException()
        {
            //Arrange
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);
            var wave = new Wave { Id = Guid.Empty, Name = "Invalid" };

            //Act
            Func<Task> act = () => repo.UpdateAsync(wave);


            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("*not valid*");
        }

        [Fact]
        public async Task UpdateAsync_WhenWaveNotFound_ShouldThrowKeyNotFoundException()
        {
            //Arrange
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);
            var wave = new Wave
            {
                Id = Guid.NewGuid(), // Not in DB
                Name = "Valid Name"
            };

            //Act
            Func<Task> act = () => repo.UpdateAsync(wave);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("*not found*");
        }

        [Fact]
        public async Task UpdateAsync_WhenNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new WaveRepository(context);

            var existingWave = new Wave { Name = "Before" };
            context.Waves.Add(existingWave);
            await context.SaveChangesAsync();

            var invalidUpdate = new Wave { Id = existingWave.Id, Name = "" };

            // Act
            Func<Task> act = () => repository.UpdateAsync(invalidUpdate);

            // Assert
            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("*name*");
        }
    }
}