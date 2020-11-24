using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public class AlleleFreqProvider
    {
        public static double[] Annotate(AnnotatedVariant variant)
        {
            var position = variant.Position;
            var refAllele = variant.RefAllele;
            var altAllele = variant.AltAllele;
            var random = position ^ refAllele.GetHashCode() ^ altAllele.GetHashCode() ^ Utilities.Prime9;

            var count = 15;
            var frequencies = new double[count];
            for (int i = 0; i < count; i++)
            {
                frequencies[i] =(random % Utilities.Prime6)*1.0/1_000_000;
                random ^= Utilities.Prime9;
            }

            return frequencies;
        }
    }
}