using System.Collections.Concurrent;
using System.Threading;
using PipeDream.VariantAnnotation;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public class ConQAnnotator
    {
        private ConcurrentQueue<AnnotatedVariant> _variants;
        private const int MaxCount = 1000;
        private SemaphoreSlim _producerSemaphore;
        private SemaphoreSlim _consumerSemaphore;
        private bool _isCancelled;
        private Thread _annoThread;
        
        public ConQAnnotator(int n)
        {
            _variants = new ConcurrentQueue<AnnotatedVariant>();
            _producerSemaphore = new SemaphoreSlim(MaxCount);
            _consumerSemaphore = new SemaphoreSlim(0);
            _annoThread = new Thread(()=> AnnotateAll());
            _annoThread.Start();
        }

        public void Complete()
        {
            _isCancelled = true;
            if (_consumerSemaphore.CurrentCount > 0)
            {
                _consumerSemaphore.Release(_consumerSemaphore.CurrentCount);
                _annoThread.Join();
            }

        }
        public void Add(AnnotatedVariant variant)
        {
            _producerSemaphore.Wait();//block if the concurrent queue has reached MaxCount
            _variants.Enqueue(variant);
            _consumerSemaphore.Release();
        }

        private void AnnotateAll()
        {
            while (true)
            {
                _consumerSemaphore.Wait();
                if (!_variants.TryDequeue(out var variant))
                {
                    if (_isCancelled) break;
                    continue;
                }
                CoreAnnotationProvider.Annotate(variant);
                SuppAnnotationProvider.Annotate(variant);
                _producerSemaphore.Release();
            }
            
        }
    }
}