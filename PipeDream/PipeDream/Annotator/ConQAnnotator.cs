using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public class ConQAnnotator
    {
        private readonly ConcurrentQueue<AnnotatedVariant> _coreQueue;
        private readonly ConcurrentQueue<AnnotatedVariant> _alleleFreqQueue;
        private readonly ConcurrentQueue<AnnotatedVariant> _idQueue;
        private readonly ConcurrentQueue<AnnotatedVariant> _clinicalQueue;
        private const int MaxCount = 1000;
        
        private readonly SemaphoreSlim _coreConsumer;
        private readonly SemaphoreSlim _alleleConsumer;
        private readonly SemaphoreSlim _idConsumer;
        private readonly SemaphoreSlim _clinicalConsumer;
        
        private readonly SemaphoreSlim _coreProducer;
        private readonly SemaphoreSlim _alleleProducer;
        private readonly SemaphoreSlim _idProducer;
        private readonly SemaphoreSlim _clinicalProducer;
        
        private bool _isCancelled;
        private readonly Task _coreTask;
        private readonly Task _alleleFreqTask;
        private readonly Task _idTask;
        private readonly Task _clinicalTask;
        
        public ConQAnnotator(int n=MaxCount)
        {
            _coreQueue = new ConcurrentQueue<AnnotatedVariant>();
            _alleleFreqQueue = new ConcurrentQueue<AnnotatedVariant>();
            _idQueue = new ConcurrentQueue<AnnotatedVariant>();
            _clinicalQueue = new ConcurrentQueue<AnnotatedVariant>();
            
            _alleleProducer = new SemaphoreSlim(n);
            _coreProducer = new SemaphoreSlim(n);
            _idProducer = new SemaphoreSlim(n);
            _clinicalProducer = new SemaphoreSlim(n);
            
            _alleleConsumer = new SemaphoreSlim(0);
            _coreConsumer = new SemaphoreSlim(0);
            _idConsumer = new SemaphoreSlim(0);
            _clinicalConsumer = new SemaphoreSlim(0);
            
            _coreTask = Task.Run(CoreAnnotate);
            _alleleFreqTask = Task.Run(AddAlleleFreq);
            _idTask = Task.Run(AddIds);
            _clinicalTask = Task.Run(AddClinical);
        }

        public void Complete()
        {
            _isCancelled = true;
            if (_coreConsumer.CurrentCount > 0)
            {
                _coreConsumer.Release(_coreConsumer.CurrentCount);
                _coreTask.Wait();
            }
            if (_alleleConsumer.CurrentCount > 0)
            {
                _alleleConsumer.Release(_alleleConsumer.CurrentCount);
                _alleleFreqTask.Wait();
            }
            if (_idConsumer.CurrentCount > 0)
            {
                _idConsumer.Release(_idConsumer.CurrentCount);
                _idTask.Wait();
            }
            if (_clinicalConsumer.CurrentCount > 0)
            {
                _clinicalConsumer.Release(_clinicalConsumer.CurrentCount);
                _clinicalTask.Wait();
            }

        }
        public void Add(AnnotatedVariant variant)
        {
            var coreSubmit = Task.Run(() =>
            {
                _coreProducer.Wait();
                _coreQueue.Enqueue(variant);
                _coreConsumer.Release();
    
            });
            
            var alleleSubmit = Task.Run(() =>
            {
                _alleleProducer.Wait();
                _alleleFreqQueue.Enqueue(variant);
                _alleleConsumer.Release();
            });
            
            var idSubmit = Task.Run(() =>
            {
                _idProducer.Wait();
                _idQueue.Enqueue(variant);
                _idConsumer.Release();

            });
            var clinicalSubmit = Task.Run(() =>
            {
                _clinicalProducer.Wait();
                _clinicalQueue.Enqueue(variant);
                _clinicalConsumer.Release();

            });
            
            coreSubmit.Wait();
            alleleSubmit.Wait();
            idSubmit.Wait();
            clinicalSubmit.Wait();
        }

        private void CoreAnnotate()
        {
            while (true)
            {
                _coreConsumer.Wait();
                if (!_coreQueue.TryDequeue(out var variant))
                {
                    if (_isCancelled) break;
                    continue;
                }
                CoreAnnotationProvider.Annotate(variant);
                _coreProducer.Release();
            }
            
        }
        
        private void AddAlleleFreq()
        {
            while (true)
            {
                _alleleConsumer.Wait();
                if (!_alleleFreqQueue.TryDequeue(out var variant))
                {
                    if (_isCancelled) break;
                    continue;
                }
                AlleleFreqProvider.Annotate(variant);
                _alleleProducer.Release();
            }
            
        }
        private void AddIds()
        {
            while (true)
            {
                _idConsumer.Wait();
                if (!_idQueue.TryDequeue(out var variant))
                {
                    if (_isCancelled) break;
                    continue;
                }
                VariantIdProvider.Annotate(variant);
                _idProducer.Release();
            }
            
        }
        
        private void AddClinical()
        {
            while (true)
            {
                _clinicalConsumer.Wait();
                if (!_clinicalQueue.TryDequeue(out var variant))
                {
                    if (_isCancelled) break;
                    continue;
                }
                ClinicalAnnotationProvider.Annotate(variant);
                _clinicalProducer.Release();
            }
            
        }
    }
}