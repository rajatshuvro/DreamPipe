using System.Threading;
using PipeDream.VariantAnnotation;

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
            _coreThread.Abort();
            _saThread.Abort();
        }

        private void CoreAnnotate()
        {
            while (true)
            {
                _coreSemaphore.Wait();
                CoreAnnotationProvider.Annotate(_variant);
                _coreDone.Wait();
            }
        }
        
        private void SaAnnotate()
        {
            while (true)
            {
                _saSemaphore.Wait();
                SuppAnnotationProvider.Annotate(_variant);
                _saDone.Wait();
            }
        }

        public void Annotate(AnnotatedVariant variant)
        {
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