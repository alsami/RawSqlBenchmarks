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

|                                Method | AmountOfMovies |         Mean |       Error |      StdDev |       Median |   Allocated |
|-------------------------------------- |--------------- |-------------:|------------:|------------:|-------------:|------------:|
|                           DapperQuery |              1 |     283.4 us |     7.58 us |    22.34 us |     283.5 us |    17.73 KB |
|                               EfQuery |              1 |     565.1 us |    27.43 us |    80.89 us |     528.8 us |     94.7 KB |
|           EfQueryPooledNoThreadSafety |              1 |     357.1 us |     7.65 us |    22.45 us |     355.9 us |    17.73 KB |
|             EfQueryPooledThreadSafety |              1 |     362.3 us |     8.15 us |    23.89 us |     364.1 us |    17.73 KB |
|                     EfQueryNoTracking |              1 |     481.2 us |     9.62 us |    17.34 us |     478.3 us |     93.9 KB |
| EfQueryNoTrackingNoThreadSafetyPooled |              1 |     371.1 us |    10.01 us |    29.03 us |     372.2 us |    17.39 KB |
|   EfQueryNoTrackingThreadSafetyPooled |              1 |     370.0 us |    10.60 us |    30.91 us |     372.2 us |    17.39 KB |
|                           DapperQuery |             10 |     346.3 us |     6.86 us |    13.85 us |     345.9 us |    28.77 KB |
|                               EfQuery |             10 |     772.5 us |    21.69 us |    63.94 us |     770.4 us |   124.14 KB |
|           EfQueryPooledNoThreadSafety |             10 |     500.3 us |     9.90 us |    24.83 us |     502.8 us |     44.2 KB |
|             EfQueryPooledThreadSafety |             10 |     489.3 us |     9.75 us |    24.27 us |     490.8 us |     44.2 KB |
|                     EfQueryNoTracking |             10 |     729.0 us |    27.24 us |    80.33 us |     741.4 us |   115.49 KB |
| EfQueryNoTrackingNoThreadSafetyPooled |             10 |     469.3 us |     9.87 us |    28.63 us |     469.1 us |    36.99 KB |
|   EfQueryNoTrackingThreadSafetyPooled |             10 |     472.7 us |     9.45 us |    23.00 us |     472.4 us |    36.98 KB |
|                           DapperQuery |            100 |     668.3 us |    13.15 us |    24.05 us |     673.8 us |   138.11 KB |
|                               EfQuery |            100 |   1,730.3 us |    34.47 us |    87.10 us |   1,748.2 us |   421.31 KB |
|           EfQueryPooledNoThreadSafety |            100 |   1,420.2 us |    28.25 us |    56.43 us |   1,414.5 us |   309.76 KB |
|             EfQueryPooledThreadSafety |            100 |   1,423.5 us |    27.28 us |    28.02 us |   1,423.5 us |   309.77 KB |
|                     EfQueryNoTracking |            100 |   1,443.0 us |    32.14 us |    94.26 us |   1,467.6 us |   331.03 KB |
| EfQueryNoTrackingNoThreadSafetyPooled |            100 |   1,107.1 us |    21.65 us |    32.41 us |   1,106.4 us |   232.41 KB |
|   EfQueryNoTrackingThreadSafetyPooled |            100 |   1,124.9 us |    20.29 us |    33.34 us |   1,125.3 us |   232.42 KB |
|                           DapperQuery |           1000 |   4,089.2 us |    80.17 us |    85.78 us |   4,091.1 us |  1229.69 KB |
|                               EfQuery |           1000 |  11,166.1 us |   168.76 us |   149.60 us |  11,131.0 us |  3452.49 KB |
|           EfQueryPooledNoThreadSafety |           1000 |  10,523.5 us |   146.18 us |   129.59 us |  10,535.5 us |   3022.7 KB |
|             EfQueryPooledThreadSafety |           1000 |  10,561.2 us |   206.22 us |   308.65 us |  10,543.9 us |   3022.7 KB |
|                     EfQueryNoTracking |           1000 |   7,917.3 us |   157.31 us |   198.95 us |   7,886.7 us |  2545.02 KB |
| EfQueryNoTrackingNoThreadSafetyPooled |           1000 |   7,457.3 us |   144.31 us |   224.68 us |   7,453.4 us |  2243.74 KB |
|   EfQueryNoTrackingThreadSafetyPooled |           1000 |   7,152.6 us |   131.72 us |   123.21 us |   7,128.3 us |  2243.74 KB |
|                           DapperQuery |          10000 |  36,816.6 us |   800.96 us | 2,361.64 us |  36,821.4 us | 12258.91 KB |
|                               EfQuery |          10000 | 113,486.6 us | 2,250.55 us | 4,281.90 us | 114,018.3 us | 33658.28 KB |
|           EfQueryPooledNoThreadSafety |          10000 | 124,596.7 us | 2,432.68 us | 4,064.47 us | 124,378.1 us | 30138.76 KB |
|             EfQueryPooledThreadSafety |          10000 | 124,495.4 us | 1,755.48 us | 1,642.08 us | 124,463.6 us | 30138.66 KB |
|                     EfQueryNoTracking |          10000 |  85,106.5 us | 1,668.39 us | 2,646.24 us |  84,885.4 us | 24749.12 KB |
| EfQueryNoTrackingNoThreadSafetyPooled |          10000 |  73,136.4 us | 1,298.44 us | 1,545.70 us |  72,982.3 us | 22422.33 KB |
|   EfQueryNoTrackingThreadSafetyPooled |          10000 |  76,004.4 us | 1,515.63 us | 2,447.47 us |  75,841.5 us | 22422.36 KB |


