using BenchmarkDotNet.Running;
using RawSqlBenchmarks;

BenchmarkRunner.Run<MovieBenchmarks>();

// Movie CreateMovie()
// {
//     return new Movie()
//     {
//         Id = Guid.NewGuid(),
//         Title = $"{Guid.NewGuid()}",
//         Description =
//             "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam porta purus dictum porta ultrices. In tincidunt neque in massa bibendum sagittis in in metus. Donec eu massa sapien. Proin fringilla tellus ut pharetra elementum. Fusce fringilla risus odio, quis bibendum justo tincidunt eu. Cras pellentesque condimentum convallis. Fusce eget ipsum sed risus rutrum porta. Sed auctor cursus risus, ut laoreet.",
//         Genre = "SomeGenre",
//         ReleaseAt = DateTimeOffset.Now
//     };
// }
//
// async Task StoreAsFile(string fileName, params Movie[] movies)
// {
//     var json = JsonSerializer.Serialize(movies);
//     await using var stream = new FileStream(Path.Combine(AppContext.BaseDirectory, "DataFiles", fileName),
//         FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
//     await stream.WriteAsync(Encoding.UTF8.GetBytes(json));
// }
//
// await StoreAsFile("1_Movies.json", CreateMovie());
// await StoreAsFile("10_Movies.json", Enumerable.Range(1, 10).Select(_ => CreateMovie()).ToArray());
// await StoreAsFile("100_Movies.json", Enumerable.Range(1, 100).Select(_ => CreateMovie()).ToArray());
// await StoreAsFile("1000_Movies.json", Enumerable.Range(1, 1000).Select(_ => CreateMovie()).ToArray());
// await StoreAsFile("10000_Movies.json", Enumerable.Range(1, 10000).Select(_ => CreateMovie()).ToArray());