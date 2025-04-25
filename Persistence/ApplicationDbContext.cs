using System.Diagnostics;
using System.Linq.Expressions;
using Domain.Abstract;
using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
    {
        public DbSet<Table1> Table1 { get; set; }
        public DbSet<Table2> Table2 { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(log =>
                {
                    var stopwatch = Stopwatch.StartNew();
                    Console.WriteLine(log);
                    stopwatch.Stop();
                    Console.WriteLine($"Query executed in {stopwatch.ElapsedMilliseconds} ms");
                }, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                #region Foreign Key Delete Behavior Configuration

                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    if (foreignKey.DeleteBehavior != DeleteBehavior.Cascade)
                    {
                        foreignKey.DeleteBehavior = (DeleteBehavior.Cascade);
                    }
                }
                #endregion

                #region Set HasQueryFilter For Deleted Entities

                if (entityType.ClrType.GetInterface(nameof(IDeletable)) != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var body = Expression.Equal(
                        Expression.Property(parameter, "IsDeleted"),
                        Expression.Constant(false, typeof(bool))
                    );
                    var lambda = Expression.Lambda(body, parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }

                #endregion

                #region Thêm Index để Filter theo ModifiedOn
                if (typeof(IModifiable).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                        .HasIndex("ModifiedOn")
                        .HasDatabaseName($"IX_{entityType.ClrType.Name}_ModifiedOn");
                }
                #endregion

                #region Add index GIN text-full-search postgresql

                #endregion
                builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            }
        }
    }
}
