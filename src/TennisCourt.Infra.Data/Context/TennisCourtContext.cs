using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using TennisCourt.Infra.CrossCutting.Commons.Providers;

namespace TennisCourt.Infra.Data.Context
{
    public class TennisCourtContext : DbContext
    {
        private readonly UserProvidedSettingsProvider _userProvided;

        protected const int DefaultVarcharLenght = 2000;

        public TennisCourtContext(DbContextOptions<TennisCourtContext> options,
                                  IOptions<UserProvidedSettingsProvider> userProvided)
           : base(options)
        {
            _userProvided = userProvided.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _userProvided.ConnectionString;
                var regexMatch = Regex.Match(connectionString,
                                            @"(?<=Initial Catalog=)([A-Za-z0-9_.]+)",
                                            RegexOptions.IgnoreCase);
                var databaseName = regexMatch.Value;

                optionsBuilder.UseInMemoryDatabase(databaseName);

                //optionsBuilder.UseSqlServer(connectionString,
                //    sqlServerOptionsAction: sqlOptions =>
                //    {
                //        sqlOptions.EnableRetryOnFailure(
                //        maxRetryCount: 10,
                //        maxRetryDelay: TimeSpan.FromSeconds(30),
                //        errorNumbersToAdd: null);
                //        sqlOptions.CommandTimeout(60);
                //    });
                //optionsBuilder.EnableSensitiveDataLogging(true);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Model.GetEntityTypes().ToList().ForEach(entityType =>
            {
                entityType.SetTableName(entityType.DisplayName());

                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);

                entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(string))
                    .Select(p => modelBuilder.Entity(p.DeclaringEntityType.ClrType).Property(p.Name))
                    .ToList()
                    .ForEach(property =>
                    {
                        property.IsUnicode(false);
                        property.HasMaxLength(DefaultVarcharLenght);
                    });
            });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TennisCourtContext).Assembly);
        }

    }
}