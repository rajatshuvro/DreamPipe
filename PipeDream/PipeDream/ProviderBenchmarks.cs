using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PipeDream.VariantAnnotation;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

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
        public void AlleleFreqAnnotations()
        {
            foreach (var variant in Variants)
            {
                AlleleFreqProvider.Annotate(variant);
            }
        }
        
        [Benchmark]
        public void IdAnnotations()
        {
            foreach (var variant in Variants)
            {
                VariantIdProvider.Annotate(variant);
            }
        }
        [Benchmark]
        public void ClinicalAnnotations()
        {
            foreach (var variant in Variants)
            {
                ClinicalAnnotationProvider.Annotate(variant);
            }
        }
    }
}