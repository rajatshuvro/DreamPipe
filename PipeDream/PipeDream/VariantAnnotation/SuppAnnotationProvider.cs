using System.Threading;

namespace PipeDream.VariantAnnotation
{
    public static class SuppAnnotationProvider
    {
        private static byte _count;
        public static void Annotate(AnnotatedVariant variant)
        {
            _count++;
            if (_count == 7)
            {
                _count = 0;
                Thread.Sleep(1);
            }

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