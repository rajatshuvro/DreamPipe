
using System.Threading;
using System.Threading.Tasks;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public class ParallelAnnotator
    {
        private readonly SemaphoreSlim _coreSemaphore;
        private readonly SemaphoreSlim _alleleFreqSemaphore;
        private readonly SemaphoreSlim _idSemaphore;
        private readonly SemaphoreSlim _clinicalSemaphore;
        
        private readonly SemaphoreSlim _coreDone;
        private readonly SemaphoreSlim _alleleFreqDone;
        private readonly SemaphoreSlim _idDone;
        private readonly SemaphoreSlim _clinicalDone;

        private AnnotatedVariant _variant;

        private readonly Task _coreTask;
        private readonly Task _alleleFreqTask;
        private readonly Task _idTask;
        private readonly Task _clinicalTask;

        private bool _isComplete;
        public ParallelAnnotator()
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
            _isComplete = true;
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
                if (_isComplete) break;
                CoreAnnotationProvider.Annotate(_variant);
                _coreDone.Release();
            }
        }
        
        private void AddAlleleFrequencies()
        {
            while (true)
            {
                _alleleFreqSemaphore.Wait();
                if (_isComplete) break;
                AlleleFreqProvider.Annotate(_variant);
                _alleleFreqSemaphore.Release();
            }
        }
        
        private void AddIds()
        {
            while (true)
            {
                _idSemaphore.Wait();
                if (_isComplete) break;
                VariantIdProvider.Annotate(_variant);
                _idSemaphore.Release();
            }
        }
        
        private void AddClinicalAnno()
        {
            while (true)
            {
                _clinicalSemaphore.Wait();
                if (_isComplete) break;
                ClinicalAnnotationProvider.Annotate(_variant);
                _clinicalSemaphore.Release();
            }
        }

        public void Annotate(AnnotatedVariant variant)
        {
            if (_isComplete) return;
            _variant = variant;
            
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