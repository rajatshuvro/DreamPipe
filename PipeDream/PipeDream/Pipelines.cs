using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PipeDream.Annotator;
using PipeDream.VariantAnnotation;

namespace PipeDream
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MemoryDiagnoser]
    public class Pipelines
    {
        private static readonly List<AnnotatedVariant> Variants = VariantAnnotation.Utilities.GetVariants(100_000);
        private static readonly List<AnnotatedVariant> ParallelVariants = VariantAnnotation.Utilities.DeepCopy(Variants);
        [Benchmark(Baseline = true)]
        public void SerialAnnotation()
        {
            var fileName = "/Users/rroy1/development/TestDatasets/outputs/SerialAnnotation.json";
            var fileStream = File.Create(fileName);
            using (var writer = new BinaryWriter(fileStream))
            {
                foreach (var variant in Variants)
                {
                    SerialAnnotator.Annotate(variant);
                    writer.Write(Utf8Json.JsonSerializer.Serialize(variant));
                }
            }
        }

        [Benchmark]
        public void ParallelAnnotation()
        {
            var fileName = "/Users/rroy1/development/TestDatasets/outputs/ParallelAnnotation.json";
            var fileStream = File.Create(fileName);
            using (var writer = new BinaryWriter(fileStream))
            {
                var parallelAnnotator = new ParallelAnnotator();
                foreach (var variant in ParallelVariants)
                {
                    parallelAnnotator.Annotate(variant);
                    writer.Write(Utf8Json.JsonSerializer.Serialize(variant));
                }
                parallelAnnotator.Complete();
            }
        }
    }
}