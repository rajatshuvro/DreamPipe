using System.Threading;

namespace PipeDream.VariantAnnotation
{
    public static class SuppAnnotationProvider
    { 
        public static void Annotate(AnnotatedVariant variant)
        {
            Thread.Sleep(1);
            var n = variant.Position % 19;
            variant.SuppAnnotations = new SuppAnnotation[n];
            var random = variant.Position;
            for (int i = 0; i < n; i++)
            {
                variant.SuppAnnotations[i] = SuppAnnotation.Create(random);
                random ^= Utilities.Prime9;
            }
            
        }
    }
}