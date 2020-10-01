using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PipeDream.VariantAnnotation;

namespace PipeDream
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MemoryDiagnoser]

    public class ProviderBenchmarks
    {
        private static readonly List<AnnotatedVariant> Variants = VariantAnnotation.Utilities.GetVariants(100_000);

        [Benchmark(Baseline = true)]
        public void CoreAnnotations()
        {
            foreach (var variant in Variants)
            {
                CoreAnnotationProvider.Annotate(variant);
            }
        }

        [Benchmark]
        public void SaAnnotations()
        {
            foreach (var variant in Variants)
            {
                SuppAnnotationProvider.Annotate(variant);
            }
        }
    }
}