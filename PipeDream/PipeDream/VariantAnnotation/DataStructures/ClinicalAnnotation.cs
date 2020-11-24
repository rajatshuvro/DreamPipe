namespace PipeDream.VariantAnnotation.DataStructures
{
    public class ClinicalAnnotation
    {
        public readonly string Pathogenicity;
        public readonly int[] PubMedIds;
        public readonly string ReviewStatus;

        public ClinicalAnnotation(string pathogenicity, int[] pubMedIds, string reviewStatus)
        {
            Pathogenicity = pathogenicity;
            PubMedIds = pubMedIds;
            ReviewStatus = reviewStatus;
        }
    }
}