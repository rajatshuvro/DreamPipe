using PipeDream.VariantAnnotation;

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