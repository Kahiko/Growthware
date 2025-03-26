```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
12th Gen Intel Core i7-12700KF, 1 CPU, 20 logical and 12 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2

Job=.NET 9.0  Runtime=.NET 9.0  

```
| Method          | Mean     | Error     | StdDev    | Median   | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|---------------- |---------:|----------:|----------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
| GetProfile      | 1.126 ms | 0.0767 ms | 0.2251 ms | 1.058 ms |  1.04 |    0.28 | 11.7188 |      - | 168.52 KB |        1.00 |
| GetProfileAsync | 1.296 ms | 0.0192 ms | 0.0180 ms | 1.293 ms |  1.19 |    0.21 | 27.3438 | 7.8125 | 366.64 KB |        2.18 |
