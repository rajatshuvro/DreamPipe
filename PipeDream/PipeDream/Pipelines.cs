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
        public const string SerialJson = "/Users/rroy1/development/TestDatasets/outputs/SerialAnnotation.json";
        public const string ParallelJson = "/Users/rroy1/development/TestDatasets/outputs/ParallelAnnotation.json";
        public const string BatchJson = "/Users/rroy1/development/TestDatasets/outputs/BatchParallelAnnotation.json";
        
        private static readonly List<AnnotatedVariant> Variants = VariantAnnotation.Utilities.GetVariants(100_000);
        [Benchmark(Baseline = true)]
        public void SerialAnnotation()
        {
            var fileName = SerialJson;
            var fileStream = File.Create(fileName);
            using (var writer = new BinaryWriter(fileStream))
            {
                foreach (var variant in Variants)
                {
                    var copyVariant = AnnotatedVariant.Create(variant.Position);
                    SerialAnnotator.Annotate(copyVariant);
                    writer.Write(Utf8Json.JsonSerializer.Serialize(copyVariant));
                }
            }
        }

        [Benchmark]
        public void ParallelAnnotation()
        {
            var fileName = ParallelJson;
            var fileStream = File.Create(fileName);
            using (var writer = new BinaryWriter(fileStream))
            {
                var parallelAnnotator = new ParallelAnnotator();
                foreach (var variant in Variants)
                {
                    var copyVariant = AnnotatedVariant.Create(variant.Position);
                    parallelAnnotator.Annotate(copyVariant);
                    writer.Write(Utf8Json.JsonSerializer.Serialize(copyVariant));
                }
                parallelAnnotator.Complete();
            }
        }
        
        [Benchmark]
        public void BatchAnnotation()
        {
            var fileName = BatchJson;
            var batchSize = 1000;
            var variantBatch = new List<AnnotatedVariant>(batchSize);
            var fileStream = File.Create(fileName);
            using (var writer = new BinaryWriter(fileStream))
            {
                var annotator = new BatchAnnotator();
                foreach (var variant in Variants)
                {
                    var copyVariant = AnnotatedVariant.Create(variant.Position);
                    variantBatch.Add(copyVariant);
                    
                    if (variantBatch.Count < batchSize) continue;
                    annotator.Annotate(variantBatch);
                    foreach (var item in variantBatch)
                    {
                        writer.Write(Utf8Json.JsonSerializer.Serialize(item));    
                    }
                    variantBatch.Clear();
                }
                //annotate last batch
                annotator.Annotate(variantBatch);
                foreach (var item in variantBatch)
                {
                    writer.Write(Utf8Json.JsonSerializer.Serialize(item));    
                }
                annotator.Complete();
            }
        }
    }
}