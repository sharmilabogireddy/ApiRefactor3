using ApiRefactor.Models;
using ApiRefactor.Repositories;
using ApiRefactor.Repositories.Interfaces;
using ApiRefactor.Test.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using ApiRefactor.Data.Contexts;

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

            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);

            var wave = new Wave { Name = "Wave A" };

            var result = await repo.SaveAsync(wave);

            Assert.NotNull(result);
            Assert.Equal("Wave A", result.Name);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task SaveAsync_Should_Throw_When_Name_Is_Empty()
        {
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);

            var wave = new Wave { Name = "" };

            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() =>
                repo.SaveAsync(wave));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Wave()
        {
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);

            var wave = new Wave { Name = "Original" };
            var saved = await repo.SaveAsync(wave);

            saved.Name = "Updated";
            var updated = await repo.UpdateAsync(saved);

            Assert.Equal("Updated", updated.Name);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Id_Empty()
        {
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);

            var wave = new Wave { Id = Guid.Empty, Name = "Invalid" };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => repo.UpdateAsync(wave));
            Assert.Contains("not valid", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Wave_Not_Found()
        {
            using var context = CreateInMemoryContext();
            var repo = new WaveRepository(context);

            var wave = new Wave
            {
                Id = Guid.NewGuid(), // Not in DB
                Name = "Valid Name"
            };

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(wave));
            Assert.Contains("not found", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Name_Is_Empty()
        {
            using var context = CreateInMemoryContext();

            var existingWave = new Wave
            {
                Id = Guid.NewGuid(),
                Name = "Original"
            };
            context.Waves.Add(existingWave);
            await context.SaveChangesAsync();

            var repo = new WaveRepository(context);

            var updatedWave = new Wave
            {
                Id = existingWave.Id,
                Name = "" // Invalid name
            };

            var ex = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() =>
                repo.UpdateAsync(updatedWave));

            Assert.Contains("name", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}