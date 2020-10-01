using System;
using System.Collections.Generic;

namespace PipeDream.VariantAnnotation
{
    public class SuppAnnotation
    {
        public string Pathogenicity;
        public string Id;
        public double[] AlleleFrequencies;
        public int[] PubMedIds;
        public string ReviewStatus;
        public static readonly string[] PathogenicitySet = { "benign", "likely benign", "likely pathogenic", "pathogenic", "unknown"};
        public static readonly string[] ReviewStatusSet =
        {
            "no assertion criteria provided", "no assertion provided", "reviewed by expert panel",
            "criteria provided, single submitter", "practice guideline","classified by multiple submitters",
            "criteria provided, conflicting interpretations","criteria provided, multiple submitters, no conflicts",
            "no interpretation for the single variant"
        };
        
        private SuppAnnotation(string id, double[] alleleFrequencies, string pathogenicity, int[] pubMedIds, string reviewStatus)
        {
            Id = id;
            AlleleFrequencies = alleleFrequencies;
            Pathogenicity = pathogenicity;
            PubMedIds = pubMedIds;
            ReviewStatus = reviewStatus;
        }

        public static SuppAnnotation Create(int position)
        {
            var id = $"rs{position:000000000}";
            var random = position ^ Utilities.Prime9;
            var alleleFreqCount = 15;
            var alleleFrequencies = new double[alleleFreqCount];
            for (int i = 0; i < alleleFreqCount; i++)
            {
                alleleFrequencies[i] =(random % Utilities.Prime6)*1.0/1_000_000;
                random ^= Utilities.Prime9;
            }
            
            var pathogenicity = PathogenicitySet[position % PathogenicitySet.Length];
            var ReviewStatus = ReviewStatusSet[position % ReviewStatusSet.Length];
            var pubmedIds = new int[1 + random % 11];
            for (int i = 0; i < pubmedIds.Length; i++)
            {
                pubmedIds[i] = 1 + random % Utilities.Prime4;
                random ^= Utilities.Prime9;
            }
            
            return new SuppAnnotation(id, alleleFrequencies, pathogenicity, pubmedIds, ReviewStatus);
        }
    }
}