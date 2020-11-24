using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public class VariantIdProvider
    {
        public static string[] Annotate(AnnotatedVariant variant)
        {
            var position = variant.Position;
            var refAllele = variant.RefAllele;
            var altAllele = variant.AltAllele;
            var random = position ^ refAllele.GetHashCode() ^ altAllele.GetHashCode() ^ Utilities.Prime9;

            var count = random % 13;
            var ids = new string[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] =$"rs{random:000000000}";
                random ^= Utilities.Prime9;
            }

            return ids;
        }
    }
}