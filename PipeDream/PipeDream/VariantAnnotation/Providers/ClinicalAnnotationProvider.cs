using System.Threading;
using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public static class ClinicalAnnotationProvider
    {
        public static readonly string[] PathogenicitySet = { "benign", "likely benign", "likely pathogenic", "pathogenic", "unknown"};
        public static readonly string[] ReviewStatusSet =
        {
            "no assertion criteria provided", "no assertion provided", "reviewed by expert panel",
            "criteria provided, single submitter", "practice guideline","classified by multiple submitters",
            "criteria provided, conflicting interpretations","criteria provided, multiple submitters, no conflicts",
            "no interpretation for the single variant"
        };

        private const byte RateLimit = 40;
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

            var pathogenicity = PathogenicitySet[random % PathogenicitySet.Length];
            var reviewStatus = ReviewStatusSet[random % ReviewStatusSet.Length];
            var pubmedIds = new int[1 + random % 11];
            for (int i = 0; i < pubmedIds.Length; i++)
            {
                pubmedIds[i] = 1 + random % Utilities.Prime4;
                random ^= Utilities.Prime9;
            }
            
            variant.ClinicalAnnotation = new ClinicalAnnotation(pathogenicity, pubmedIds, reviewStatus);
        }

    }
}