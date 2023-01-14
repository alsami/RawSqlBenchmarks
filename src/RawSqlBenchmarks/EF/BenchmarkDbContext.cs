using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using RawSqlBenchmarks.Models;

namespace RawSqlBenchmarks.EF;

public class BenchmarkDbContextFactory : IDesignTimeDbContextFactory<BenchmarkDbContext>
{
    public BenchmarkDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder()
            .UseSqlServer(configuration.GetConnectionString("SqlServer"));

        return new BenchmarkDbContext(optionsBuilder.Options);
    }
}

public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movie");

        builder.HasKey(movie => movie.Id);
        
        builder
            .HasIndex(movie => movie.Title)
            .IsUnique();

        builder
            .Property(movie => movie.Title)
            .HasMaxLength(128);
        
        builder
            .Property(movie => movie.Description)
            .HasMaxLength(1024);
    }
}

public class BenchmarkDbContext : DbContext
{
    public BenchmarkDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovieEntityTypeConfiguration());
    }
}