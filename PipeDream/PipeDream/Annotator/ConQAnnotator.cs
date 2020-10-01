using System.Collections.Concurrent;
using System.Threading;
using PipeDream.VariantAnnotation;

namespace PipeDream.Annotator
{
    public class ConQAnnotator
    {
        private ConcurrentQueue<AnnotatedVariant> _variants;
        private const int MaxCount = 1000;
        private SemaphoreSlim _addVariantsSemaphore;

        public ConQAnnotator(int n)
        {
            _variants = new ConcurrentQueue<AnnotatedVariant>();
            _addVariantsSemaphore = new SemaphoreSlim(MaxCount);
        }

        public void Add(AnnotatedVariant variant)
        {
            _addVariantsSemaphore.Wait();//block if the concurrent queue has reached MaxCount
            
        }
    }
}