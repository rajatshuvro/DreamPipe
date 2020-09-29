using System;
using System.Collections.Generic;
using System.Threading;

namespace PipeDream.VariantAnnotation
{
    public static class SuppAnnotationProvider
    { 
        public static void Annotate(AnnotatedVariant variant)
        {
            Thread.Sleep(2);
            variant.SuppAnno = SuppAnnotation.Create(variant.Position);
        }
    }
}