using PipeDream.VariantAnnotation;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public static class SerialAnnotator
    {
        public static void Annotate(AnnotatedVariant variant)
        {
            CoreAnnotationProvider.Annotate(variant);
            SuppAnnotationProvider.Annotate(variant);
        }
    }
}