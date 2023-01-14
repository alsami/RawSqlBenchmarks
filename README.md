# RawSqlBenchmarks

This is a small repo to test the performance of raw-sql using `EF` vs `Dapper` with SQL Server 2019.

## Setup

1. Run `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<YOUR-PASSWORD>" -p 1433:1433 --name <YOUR_SQL_SERVER_DOCKER_IMAGE_NAME> --restart=always -d mcr.microsoft.com/mssql/server:2019-latest`
1. Update the `appsettings.json` and add your connection string: `Server=127.0.0.1,1433;Initial Catalog=RawSqlBenchmarks;Persist Security Info=False;User ID=sa;Password=<YOUR-PASSWORD>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;Max Pool Size=100;`
1. Apply the migration by opening a command-line interface within the project root (not repo root!) `dotnet ef database update`

## Execute benchmarks

1. Navigate into the project root with a command-line interface and run `dotnet run -c Release`. This will take a good amount of time so don't wait for it to finish :-)

## Notes

1. I ran the benchmark several times and `Dapper` has always been faster and allocated fewer bytes
1. This is a very simple use-case obviously, since no navigation properties are involved

## Machine configuration

```text
OS: Linux, Ubuntu 22.04
SDK: 7.0.102
CPU: AMD Ryzen 3950x
Disk: Samsung 970 Evo Plus
RAM: Corsair Vengeance LPX 64GB (2x32GB) DDR4 3200MHz C16
```

## Results

|            Method | AmountOfMovies |         Mean |       Error |      StdDev |       Median |   Allocated |
|------------------ |--------------- |-------------:|------------:|------------:|-------------:|------------:|
|       DapperQuery |              1 |     277.7 us |     5.24 us |    14.52 us |     276.4 us |    17.74 KB |
|           EfQuery |              1 |     546.8 us |    24.52 us |    72.30 us |     524.6 us |    94.66 KB |
| EfQueryNoTracking |              1 |     530.5 us |    22.34 us |    65.86 us |     500.9 us |     93.9 KB |
|       DapperQuery |             10 |     335.6 us |     7.80 us |    23.01 us |     338.5 us |    28.77 KB |
|           EfQuery |             10 |     716.3 us |    30.32 us |    89.41 us |     714.5 us |   124.08 KB |
| EfQueryNoTracking |             10 |     576.1 us |    11.08 us |    12.76 us |     580.2 us |   115.49 KB |
|       DapperQuery |            100 |     653.1 us |    12.98 us |    36.83 us |     659.1 us |   138.26 KB |
|           EfQuery |            100 |   1,749.6 us |    34.56 us |    84.78 us |   1,758.5 us |    421.2 KB |
| EfQueryNoTracking |            100 |   1,415.5 us |    31.70 us |    93.46 us |   1,444.7 us |   330.96 KB |
|       DapperQuery |           1000 |   4,074.9 us |    80.75 us |    99.16 us |   4,073.1 us |  1228.45 KB |
|           EfQuery |           1000 |  11,099.1 us |   219.63 us |   360.86 us |  11,034.0 us |  3452.46 KB |
| EfQueryNoTracking |           1000 |   8,114.9 us |   159.05 us |   212.33 us |   8,112.3 us |  2545.01 KB |
|       DapperQuery |          10000 |  37,256.3 us |   864.90 us | 2,550.17 us |  36,900.5 us | 12255.71 KB |
|           EfQuery |          10000 | 115,648.1 us | 2,289.17 us | 4,299.62 us | 114,878.0 us | 33657.57 KB |
| EfQueryNoTracking |          10000 |  78,265.6 us | 1,477.74 us | 1,382.28 us |  77,987.5 us | 24752.66 KB |

