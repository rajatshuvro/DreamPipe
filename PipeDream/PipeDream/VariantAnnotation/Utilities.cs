using System;
using System.Collections.Generic;

namespace PipeDream.VariantAnnotation
{
    public class Utilities
    {
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
    }
}