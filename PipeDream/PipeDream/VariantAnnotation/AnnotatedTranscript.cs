using System;

namespace PipeDream.VariantAnnotation
{
    public sealed class AnnotatedTranscript
    {
        public static readonly string[] ConsequenceSet =
        {
            "coding_sequence_variant", "downstream_gene_variant", "five_prime_duplicated_transcript",
            "five_prime_UTR_variant", "frameshift_variant", "incomplete_terminal_codon_variant", "start_lost",
            "intron_variant", "missense_variant", "mature_miRNA_variant", "protein_altering_variant", "splice_region_variant",
            "stop_lost", "stop_gained", "stop_retained", "synonymous_variant","transcript_variant"
        };
        public string Id;
        public string[] Consequences;
        private static readonly Random _random = new Random();

        private AnnotatedTranscript(string id, string[] consequences)
        {
            Id = id;
            Consequences = consequences;
        }

        public static AnnotatedTranscript Create(int position, int n)
        {
            var x = 1 + position % 9991;
            var id = $"TRAN_{x:0000000}.{n}";
            var consequences = new string[1 + n % 7];
            for (int i = 0; i < consequences.Length; i++)
            {
                var j = _random.Next(0, ConsequenceSet.Length - 1);
                consequences[i] = ConsequenceSet[j];
            }

            return new AnnotatedTranscript(id, ConsequenceSet);
        }
            
    }
}