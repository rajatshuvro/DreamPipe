using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PipeDream.VariantAnnotation;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public class BatchParallelAnnotator
    {
        private readonly SemaphoreSlim _coreSemaphore;
        private readonly SemaphoreSlim _alleleFreqSemaphore;
        private readonly SemaphoreSlim _idSemaphore;
        private readonly SemaphoreSlim _clinicalSemaphore;
        
        private readonly SemaphoreSlim _coreDone;
        private readonly SemaphoreSlim _alleleFreqDone;
        private readonly SemaphoreSlim _idDone;
        private readonly SemaphoreSlim _clinicalDone;
        
        private List<AnnotatedVariant> _variants;

        private readonly Task _coreTask;
        private readonly Task _alleleFreqTask;
        private readonly Task _idTask;
        private readonly Task _clinicalTask;

        private bool _isCancelled;
        public BatchParallelAnnotator()
        {
            _coreSemaphore = new SemaphoreSlim(0);
            _alleleFreqSemaphore = new SemaphoreSlim(0);
            _idSemaphore = new SemaphoreSlim(0);
            _clinicalSemaphore = new SemaphoreSlim(0);
            
            _coreDone = new SemaphoreSlim(0);
            _alleleFreqDone = new SemaphoreSlim(0);
            _idDone = new SemaphoreSlim(0);
            _clinicalDone = new SemaphoreSlim(0);
            
            _coreTask = Task.Run(CoreAnnotate);
            _alleleFreqTask = Task.Run(AddAlleleFrequencies);
            _idTask = Task.Run(AddIds);
            _clinicalTask = Task.Run(AddClinicalAnno);
        }

        public void Complete()
        {
            _isCancelled = true;
            _coreSemaphore.Release();
            _alleleFreqSemaphore.Release();
            _idSemaphore.Release();
            _clinicalSemaphore.Release();

            _coreTask.Wait();
            _alleleFreqTask.Wait();
            _idTask.Wait();
            _clinicalTask.Wait();
        }

        private void CoreAnnotate()
        {
            while (true)
            {
                _coreSemaphore.Wait();
                if (_isCancelled) break;
                foreach (var variant in _variants)
                {
                    CoreAnnotationProvider.Annotate(variant);    
                }
                
                _coreDone.Release();
            }
        }
        
        private void AddAlleleFrequencies()
        {
            while (true)
            {
                _alleleFreqSemaphore.Wait();
                if (_isCancelled) break;
                foreach (var variant in _variants)
                {
                    AlleleFreqProvider.Annotate(variant);    
                }
                
                _alleleFreqDone.Release();
            }
        }

        private void AddIds()
        {
            while (true)
            {
                _idSemaphore.Wait();
                if (_isCancelled) break;
                foreach (var variant in _variants)
                {
                    VariantIdProvider.Annotate(variant);    
                }
                
                _idSemaphore.Release();
            }
        }
        
        private void AddClinicalAnno()
        {
            while (true)
            {
                _clinicalSemaphore.Wait();
                if (_isCancelled) break;
                foreach (var variant in _variants)
                {
                    ClinicalAnnotationProvider.Annotate(variant);    
                }
                
                _clinicalSemaphore.Release();
            }
        }

        public void Annotate(List<AnnotatedVariant> variants)
        {
            if (_isCancelled) return;

            _variants = variants;
            //indicate to the threads that new variant is ready
            _coreSemaphore.Release();
            _alleleFreqSemaphore.Release();
            _idSemaphore.Release();
            _clinicalSemaphore.Release();
            
            //now wait for the annotations to be done
            _coreDone.Wait();
            _alleleFreqDone.Wait();
            _idDone.Wait();
            _clinicalDone.Wait();
        }
    }
}