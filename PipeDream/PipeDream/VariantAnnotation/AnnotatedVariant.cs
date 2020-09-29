using System;

namespace PipeDream.VariantAnnotation
{
    public class AnnotatedVariant
    {

        public static readonly string[] Alleles = {"A", "C", "G", "T", "AA", "AC", "AT", "AG", "CC", "CG", "CT", "CA", "GA", "GC", "GT", "GG","TA", "TC", "TT", "TG"};
        
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
            var refAllele = Alleles[(position*13) % Alleles.Length];
            var altAllele = Alleles[(position*17) % Alleles.Length];
            
            return new AnnotatedVariant(position, refAllele, altAllele);
        }
        
    }
}