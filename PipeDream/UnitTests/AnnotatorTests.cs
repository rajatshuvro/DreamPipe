using System;
using PipeDream.Annotator;
using PipeDream.VariantAnnotation;
using Xunit;

namespace UnitTests
{
    public class AnnotatorTests
    {
        [Fact]
        public void CompareOneOutput()
        {
            var position = 1234689;
            var variant1 = AnnotatedVariant.Create(position);
            var variant2 = AnnotatedVariant.Create(position);
            
            SerialAnnotator.Annotate(variant1);
            var serialJson = Utf8Json.JsonSerializer.ToJsonString(variant1);
            
            var parallelAnnotator = new ParallelAnnotator();
            parallelAnnotator.Annotate(variant2);
            parallelAnnotator.Complete();
            
            var parallelJson = Utf8Json.JsonSerializer.ToJsonString(variant2);
            
            Assert.Equal(serialJson, parallelJson);
        }
        
        [Fact]
        public void CompareMany()
        {
            var variants_1 = PipeDream.VariantAnnotation.Utilities.GetVariants(1000);
            var variants_2 = PipeDream.VariantAnnotation.Utilities.DeepCopy(variants_1);

            var parallelAnnotator = new ParallelAnnotator();
            for (int i = 0; i < variants_1.Count; i++)
            {
                var variant = variants_1[i];
                SerialAnnotator.Annotate(variant);
                var serialJson = Utf8Json.JsonSerializer.ToJsonString(variant);

                var variant2 = variants_2[i];
                parallelAnnotator.Annotate(variant2);
            
                var parallelJson = Utf8Json.JsonSerializer.ToJsonString(variant2);
            
                Assert.Equal(serialJson, parallelJson);
                
            }
            parallelAnnotator.Complete();

        }
    }
}