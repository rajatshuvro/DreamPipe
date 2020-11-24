using System.Threading;
using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public static class CoreAnnotationProvider
    {
        private static byte _count=0;
        public static void Annotate(AnnotatedVariant variant)
        {
            _count++;
            if (_count == 10)
            {
                _count = 0;
                Thread.Sleep(1);
            }
            
            var position = variant.Position;
            
            var n = 1 + position % 13;// that its never 0
            var transcripts = new AnnotatedTranscript[n];
            for (int i = 0; i < n; i++)
            {
                transcripts[i] = AnnotatedTranscript.Create(position, i);
            }

            variant.Transcripts = transcripts;
        }
    }
}