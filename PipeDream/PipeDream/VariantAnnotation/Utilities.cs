using System;
using System.Collections.Generic;

namespace PipeDream.VariantAnnotation
{
    public class Utilities
    {
        public const int Prime4 = 7507;
        public const int Prime6 = 978403;
        public const int Prime9 = 988724531;
        public static List<AnnotatedVariant> GetVariants(int n)
        {
            var delta = new Random();
            var variants = new List<AnnotatedVariant>(n);

            var position = delta.Next(1000, 5000);
            for (int i = 0; i < n; i++)
            {
                position += delta.Next(0, 500);
                variants.Add(AnnotatedVariant.Create(position));
            }

            return variants;
        }

        public static List<AnnotatedVariant> DeepCopy(List<AnnotatedVariant> variants)
        {
            var newVariants = new List<AnnotatedVariant>(variants.Count);
            for (int i = 0; i < variants.Count; i++)
            {
                newVariants.Add(AnnotatedVariant.Create(variants[i].Position));
            }

            return newVariants;
        }
    }
}