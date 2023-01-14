namespace RawSqlBenchmarks.Models;

public class Movie
{
    public Guid Id { get; set; }

    public DateTimeOffset ReleaseAt { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Genre { get; set; } = null!;
}