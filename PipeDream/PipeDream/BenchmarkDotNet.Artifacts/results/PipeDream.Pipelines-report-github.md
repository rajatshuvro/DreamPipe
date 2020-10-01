``` ini

BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.6 (19G2021) [Darwin 19.6.0]
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.1.102
  [Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT


```
|             Method |    Mean |    Error |   StdDev | Ratio | Rank |       Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|------------------- |--------:|---------:|---------:|------:|-----:|------------:|-----------:|----------:|----------:|
|   SerialAnnotation | 5.141 s | 0.0384 s | 0.0340 s |  1.00 |    1 | 122000.0000 |  1000.0000 |         - | 976.16 MB |
| ParallelAnnotation | 5.444 s | 0.0692 s | 0.0647 s |  1.06 |    2 | 137000.0000 |  2000.0000 |         - | 973.74 MB |
|    BatchAnnotation | 5.498 s | 0.0283 s | 0.0251 s |  1.07 |    2 | 131000.0000 | 61000.0000 | 9000.0000 | 974.54 MB |
