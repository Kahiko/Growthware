```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
12th Gen Intel Core i7-12700KF, 1 CPU, 20 logical and 12 physical cores
.NET SDK 9.0.201
  [Host]   : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2

Job=.NET 9.0  Runtime=.NET 9.0  

```
| Method          | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|---------------- |---------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| GetProfile      | 1.181 ms | 0.0228 ms | 0.0262 ms |  1.00 |    0.03 | 19.5313 | 3.9063 | 258.78 KB |        1.00 |
| GetProfileAsync | 1.596 ms | 0.0312 ms | 0.0333 ms |  1.35 |    0.04 | 35.1563 | 7.8125 | 453.34 KB |        1.75 |
