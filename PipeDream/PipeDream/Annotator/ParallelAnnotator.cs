using System.Threading;
using PipeDream.VariantAnnotation;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public class ParallelAnnotator
    {
        private SemaphoreSlim _coreSemaphore;
        private SemaphoreSlim _saSemaphore;
        private SemaphoreSlim _coreDone;
        private SemaphoreSlim _saDone;
        
        private AnnotatedVariant _variant;

        private Thread _coreThread;
        private Thread _saThread;

        private bool _isComplete;
        public ParallelAnnotator()
        {
            _coreSemaphore = new SemaphoreSlim(0);
            _saSemaphore = new SemaphoreSlim(0);
            _coreDone = new SemaphoreSlim(0);
            _saDone = new SemaphoreSlim(0);

            _coreThread = new Thread(()=> CoreAnnotate());
            _saThread = new Thread(()=> SaAnnotate());
            _coreThread.Start();
            _saThread.Start();
        }

        public void Complete()
        {
            _isComplete = true;
            _coreSemaphore.Release();
            _saSemaphore.Release();
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
        
        private void SaAnnotate()
        {
            while (true)
            {
                _saSemaphore.Wait();
                if (_isComplete) break;
                SuppAnnotationProvider.Annotate(_variant);
                _saDone.Release();
            }
        }

        public void Annotate(AnnotatedVariant variant)
        {
            if (_isComplete) return;
            _variant = variant;
            //indicate to the threads that new variant is ready
            _coreSemaphore.Release();
            _saSemaphore.Release();
            
            //now wait for the annotations to be done
            _coreDone.Wait();
            _saDone.Wait();
        }
    }
}