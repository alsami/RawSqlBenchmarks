using System.Data.SqlClient;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using RawSqlBenchmarks.EF;
using RawSqlBenchmarks.Models;

namespace RawSqlBenchmarks;

[MemoryDiagnoser(false)]
public class MovieBenchmarks
{
    private const string Query = "SELECT * FROM Movie"; 
    private readonly IConfiguration _configuration;
    private readonly PooledDbContextFactory<BenchmarkDbContext> _pooledDbContextFactoryNoThreadSafety;
    private readonly PooledDbContextFactory<BenchmarkDbContext> _pooledDbContextFactoryThreadSafety;

    public MovieBenchmarks()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .Build();


        _pooledDbContextFactoryNoThreadSafety = new PooledDbContextFactory<BenchmarkDbContext>(
            new DbContextOptionsBuilder<BenchmarkDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("SqlServer"))
                .EnableThreadSafetyChecks(false)
                .Options);
        
        _pooledDbContextFactoryThreadSafety = new PooledDbContextFactory<BenchmarkDbContext>(
            new DbContextOptionsBuilder<BenchmarkDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("SqlServer"))
                .EnableThreadSafetyChecks(false)
                .Options); 
    }
    
    [Params(1, 10, 100, 1000, 10000)]
    public int AmountOfMovies { get; set; }

    [GlobalSetup]
    public async ValueTask Initialize()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "DataFiles",
            $"{AmountOfMovies}_Movies.json");

        var moviesJson = await File.ReadAllTextAsync(path);
        var movies = JsonSerializer.Deserialize<Movie[]>(moviesJson)!;
        var context = new BenchmarkDbContextFactory().CreateDbContext(Array.Empty<string>());

        await context.Set<Movie>().AddRangeAsync(movies);
        await context.SaveChangesAsync();
    }

    [GlobalCleanup]
    public async ValueTask CleanupAsync()
    {
        var context = new BenchmarkDbContextFactory().CreateDbContext(Array.Empty<string>());
        await context.Set<Movie>().Where(_ => true).ExecuteDeleteAsync();
    }

    [Benchmark]
    public async ValueTask DapperQuery()
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        var movies = (await connection.QueryAsync<Movie>(Query)).ToArray();
        movies.Should().NotBeEmpty();
    }

    [Benchmark]
    public async ValueTask EfQuery()
    {
        await using var context = new BenchmarkDbContextFactory().CreateDbContext(Array.Empty<string>());
        var movies = await context.Set<Movie>().FromSqlRaw(Query).ToArrayAsync();
        movies.Should().NotBeEmpty();
    }
    
    [Benchmark]
    public async ValueTask EfQueryPooledNoThreadSafety()
    {
        await using var context = await _pooledDbContextFactoryNoThreadSafety.CreateDbContextAsync(); 
        var movies = await context.Set<Movie>().FromSqlRaw(Query).ToArrayAsync();
        movies.Should().NotBeEmpty();
    }
    
    [Benchmark]
    public async ValueTask EfQueryPooledThreadSafety()
    {
        await using var context = await _pooledDbContextFactoryThreadSafety.CreateDbContextAsync(); 
        var movies = await context.Set<Movie>().FromSqlRaw(Query).ToArrayAsync();
        movies.Should().NotBeEmpty();
    }
    
    [Benchmark]
    public async ValueTask EfQueryNoTracking()
    {
        var context = new BenchmarkDbContextFactory().CreateDbContext(Array.Empty<string>());
        var movies = await context.Set<Movie>().FromSqlRaw(Query).AsNoTracking().ToArrayAsync();
        movies.Should().NotBeEmpty();
    }
    
    [Benchmark]
    public async ValueTask EfQueryNoTrackingNoThreadSafetyPooled()
    {
        await using var context = await _pooledDbContextFactoryNoThreadSafety.CreateDbContextAsync(); 
        var movies = await context.Set<Movie>().FromSqlRaw(Query).AsNoTracking().ToArrayAsync();
        movies.Should().NotBeEmpty();
    }
    
    [Benchmark]
    public async ValueTask EfQueryNoTrackingThreadSafetyPooled()
    {
        await using var context = await _pooledDbContextFactoryThreadSafety.CreateDbContextAsync(); 
        var movies = await context.Set<Movie>().FromSqlRaw(Query).AsNoTracking().ToArrayAsync();
        movies.Should().NotBeEmpty();
    }
}