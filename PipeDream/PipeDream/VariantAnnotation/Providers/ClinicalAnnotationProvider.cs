using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public class ClinicalAnnotationProvider
    {
        public static readonly string[] PathogenicitySet = { "benign", "likely benign", "likely pathogenic", "pathogenic", "unknown"};
        public static readonly string[] ReviewStatusSet =
        {
            "no assertion criteria provided", "no assertion provided", "reviewed by expert panel",
            "criteria provided, single submitter", "practice guideline","classified by multiple submitters",
            "criteria provided, conflicting interpretations","criteria provided, multiple submitters, no conflicts",
            "no interpretation for the single variant"
        };

        public static ClinicalAnnotation Annotate(AnnotatedVariant variant)
        {
            var position = variant.Position;
            var refAllele = variant.RefAllele;
            var altAllele = variant.AltAllele;
            var random = position ^ refAllele.GetHashCode() ^ altAllele.GetHashCode() ^ Utilities.Prime9;

            var pathogenicity = PathogenicitySet[random % PathogenicitySet.Length];
            var reviewStatus = ReviewStatusSet[random % ReviewStatusSet.Length];
            var pubmedIds = new int[1 + random % 11];
            for (int i = 0; i < pubmedIds.Length; i++)
            {
                pubmedIds[i] = 1 + random % Utilities.Prime4;
                random ^= Utilities.Prime9;
            }
            
            return new ClinicalAnnotation(pathogenicity, pubmedIds, reviewStatus);
        }

    }
}