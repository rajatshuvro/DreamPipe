using System;

namespace PipeDream.VariantAnnotation
{
    public class AnnotatedVariant
    {

        public static readonly string[] Alleles = {"A", "C", "G", "T", "AA", "AC", "AT", "AG", "CC", "CG", "CT", "CA", "GA", "GC", "GT", "GG","TA", "TC", "TT", "TG"};
        private static Random _random = new Random();
        
        public readonly int Position;
        public string RefAllele ;
        public string AltAllele;

        public AnnotatedTranscript[] Transcripts;

        public SuppAnnotation SuppAnno;

        private AnnotatedVariant( int position, string refAllele, string altAllele)
        {
            Position = position;
            RefAllele = refAllele;
            AltAllele = altAllele;
        }

        public static AnnotatedVariant Create(int position)
        {
            var refAllele = Alleles[_random.Next(0, Alleles.Length - 1)];
            var altAllele = Alleles[_random.Next(0, Alleles.Length - 1)];
            
            return new AnnotatedVariant(position, refAllele, altAllele);
        }
        
    }
}