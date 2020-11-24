using System.Threading;
using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public static class VariantIdProvider
    {
        private const byte RateLimit = 60;
        private static byte _countToDelay= RateLimit;
        public static void Annotate(AnnotatedVariant variant)
        {
            _countToDelay--;
            if (_countToDelay == 0)
            {
                _countToDelay = RateLimit;
                Thread.Sleep(1);
            }

            var position = variant.Position;
            var refAllele = variant.RefAllele;
            var altAllele = variant.AltAllele;
            var random = position ^ refAllele.GetHashCode() ^ altAllele.GetHashCode() ^ Utilities.Prime9;
            if (random < 0) random = -random;
            
            var count = random % 13+1;
            var ids = new string[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] =$"rs{random:0000000000}";
                random ^= Utilities.Prime9;
            }

            variant.Ids = ids;
        }
    }
}