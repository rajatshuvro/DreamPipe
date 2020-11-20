using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PipeDream.Annotator;
using PipeDream.VariantAnnotation;
using Xunit;

namespace UnitTests
{
    public class AnnotatorTests
    {
        [Fact]
        public void CompareOneOutput_parallel()
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
        public void CompareOneOutput_channel()
        {
            var position = 1234689;
            var variant1 = AnnotatedVariant.Create(position);
            var variant2 = AnnotatedVariant.Create(position);
            
            SerialAnnotator.Annotate(variant1);
            var serialJson = Utf8Json.JsonSerializer.ToJsonString(variant1);
            
            var channelAnnotator = new ChannelAnnotator();
            channelAnnotator.Submit(variant2);
            channelAnnotator.Complete();
            
            var parallelJson = Utf8Json.JsonSerializer.ToJsonString(variant2);
            
            Assert.Equal(serialJson, parallelJson);
        }
        
        [Fact]
        public void CompareOneOutput_conQ()
        {
            var position = 1234689;
            var variant1 = AnnotatedVariant.Create(position);
            var variant2 = AnnotatedVariant.Create(position);
            
            SerialAnnotator.Annotate(variant1);
            var serialJson = Utf8Json.JsonSerializer.ToJsonString(variant1);

            var conQAnnotator = new ConQAnnotator(50);
            conQAnnotator.Add(variant2);
            conQAnnotator.Complete();
            
            var conqJson = Utf8Json.JsonSerializer.ToJsonString(variant2);
            
            Assert.Equal(serialJson, conqJson);
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
        
        [Fact]
        public void CompareMany_conQ()
        {
            var count = 1000;
            var variants_1 = PipeDream.VariantAnnotation.Utilities.GetVariants(count);
            var variants_2 = PipeDream.VariantAnnotation.Utilities.DeepCopy(variants_1);

            var annotator = new ConQAnnotator(count > 10? count/10: count);
            foreach (var variant in variants_1)
            {
                SerialAnnotator.Annotate(variant);
            }
            
            foreach (var variant in variants_2)
            {
                annotator.Add(variant);
            }
            annotator.Complete();

            for (int i = 0; i < variants_1.Count; i++)
            {
                var serialJson = Utf8Json.JsonSerializer.ToJsonString(variants_1[i]);
                var parallelJson = Utf8Json.JsonSerializer.ToJsonString(variants_2[i]);
            
                Assert.Equal(serialJson, parallelJson);

            }

        }
        
        [Fact]
        public void CompareMany_channel()
        {
            var count = 1000;
            var variants_1 = PipeDream.VariantAnnotation.Utilities.GetVariants(count);
            var variants_2 = PipeDream.VariantAnnotation.Utilities.DeepCopy(variants_1);

            var annotator = new ChannelAnnotator();
            foreach (var variant in variants_1)
            {
                SerialAnnotator.Annotate(variant);
            }

            Task.Run(async () =>
            {
                foreach (var variant in variants_2)
                {
                    await annotator.Submit(variant);
                }    
            });
            
            Thread.Sleep(50);
            annotator.Complete();

            for (int i = 0; i < variants_1.Count; i++)
            {
                var serialJson = Utf8Json.JsonSerializer.ToJsonString(variants_1[i]);
                var parallelJson = Utf8Json.JsonSerializer.ToJsonString(variants_2[i]);
            
                Assert.Equal(serialJson, parallelJson);

            }

        }
    }
}