using System;
using System.Collections.Generic;

namespace PipeDream.VariantAnnotation
{
    public static class SuppAnnotationProvider
    { 
        public static void Annotate(AnnotatedVariant variant)
        {
            variant.SuppAnno = SuppAnnotation.Create(variant.Position);
        }
    }
}