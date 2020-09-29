using System;

namespace PipeDream.VariantAnnotation
{
    public class SuppAnnotation
    {
        public string Pathogenicity;
        public double AlleleFrequency;
        public int[] PubMedIds;
        public static readonly string[] PathogenicitySet = { "benign", "likely benign", "likely pathogenic", "pathogenic", "unknown"};

        private static readonly Random _random = new Random();

        private SuppAnnotation(double alleleFrequency, string pathogenicity, int[] pubMedIds)
        {
            AlleleFrequency = alleleFrequency;
            Pathogenicity = pathogenicity;
            PubMedIds = pubMedIds;
        }

        public static SuppAnnotation Create(int position)
        {
            var alleleFrequency = _random.Next(0,1_000_000)*1.0/1_000_000;
            var pathogenicity = PathogenicitySet[position % PathogenicitySet.Length];
            var pubmedIds = new int[1 + position % 11];
            for (int i = 0; i < pubmedIds.Length; i++)
            {
                pubmedIds[i] = 1 + position % 7507;
            }
            
            return new SuppAnnotation(alleleFrequency, pathogenicity, pubmedIds);
        }
    }
}