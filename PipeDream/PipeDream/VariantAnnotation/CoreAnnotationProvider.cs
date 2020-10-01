using System;
using System.Collections.Generic;
using System.Threading;

namespace PipeDream.VariantAnnotation
{
    public static class CoreAnnotationProvider
    {
        public static void Annotate(AnnotatedVariant variant)
        {
            //Thread.Sleep(1);
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