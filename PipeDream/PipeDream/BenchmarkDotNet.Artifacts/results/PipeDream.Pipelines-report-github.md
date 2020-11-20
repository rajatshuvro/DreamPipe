``` ini

BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.7 (19H15) [Darwin 19.6.0]
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.1.102
  [Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT


```
|               Method |    Mean |    Error |   StdDev | Ratio | RatioSD | Rank |       Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|--------------------- |--------:|---------:|---------:|------:|--------:|-----:|------------:|-----------:|----------:|----------:|
|     SerialAnnotation | 4.364 s | 0.0868 s | 0.1033 s |  1.00 |    0.00 |    1 | 122000.0000 |  1000.0000 |         - |    975 MB |
|      BatchAnnotation | 4.517 s | 0.0891 s | 0.0833 s |  1.04 |    0.02 |    2 | 131000.0000 | 58000.0000 | 9000.0000 | 972.37 MB |
|   ParallelAnnotation | 4.889 s | 0.0943 s | 0.1192 s |  1.12 |    0.04 |    3 | 136000.0000 |  2000.0000 |         - | 973.35 MB |
| ConcurrentAnnotation | 5.210 s | 0.0846 s | 0.0791 s |  1.19 |    0.04 |    4 | 126000.0000 | 31000.0000 | 4000.0000 |    975 MB |
