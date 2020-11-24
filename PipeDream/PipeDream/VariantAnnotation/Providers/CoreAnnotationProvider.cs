using System.Threading;
using PipeDream.VariantAnnotation.DataStructures;

namespace PipeDream.VariantAnnotation.Providers
{
    public static class CoreAnnotationProvider
    {
        private const byte RateLimit = 50;
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