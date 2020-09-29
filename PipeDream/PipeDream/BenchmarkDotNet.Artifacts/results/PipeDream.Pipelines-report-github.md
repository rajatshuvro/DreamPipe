``` ini

BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.6 (19G2021) [Darwin 19.6.0]
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.1.102
  [Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT


```
|             Method |    Mean |    Error |   StdDev | Ratio | RatioSD | Rank |      Gen 0 |      Gen 1 | Gen 2 | Allocated |
|------------------- |--------:|---------:|---------:|------:|--------:|-----:|-----------:|-----------:|------:|----------:|
|   SerialAnnotation | 1.511 s | 0.0295 s | 0.0424 s |  1.00 |    0.00 |    1 | 55000.0000 | 21000.0000 |     - | 439.32 MB |
| ParallelAnnotation | 1.742 s | 0.0346 s | 0.0745 s |  1.15 |    0.07 |    2 | 59000.0000 | 15000.0000 |     - | 439.91 MB |
