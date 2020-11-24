using System.Threading.Channels;
using System.Threading.Tasks;
using PipeDream.VariantAnnotation;
using PipeDream.VariantAnnotation.DataStructures;
using PipeDream.VariantAnnotation.Providers;

namespace PipeDream.Annotator
{
    public class ChannelAnnotator
    {
        public const int DefaultSize = 50;
        private readonly Channel<AnnotatedVariant> _coreChannel;
        private readonly Channel<AnnotatedVariant> _alleleFreqChannel;
        private readonly Channel<AnnotatedVariant> _idChannel;
        private readonly Channel<AnnotatedVariant> _clinicalChannel;
        //private readonly StreamWriter _writer;
        private Task _coreTask, _alleleFreqTask, _idTask, _clinicalTask;

        public ChannelAnnotator( int size = DefaultSize)
        {
            //_writer = writer;
            _coreChannel = Channel.CreateBounded<AnnotatedVariant>(size);
            _alleleFreqChannel = Channel.CreateBounded<AnnotatedVariant>(size);
            _idChannel = Channel.CreateBounded<AnnotatedVariant>(size);
            _clinicalChannel = Channel.CreateBounded<AnnotatedVariant>(size);
            
            _coreTask =  Task.Run(CoreAnnotation);
            _alleleFreqTask = Task.Run(AddAlleleFreq);
            _idTask = Task.Run(AddIds);
            _clinicalTask = Task.Run(AddClinical);
        }

        public async ValueTask Submit(AnnotatedVariant variant)
        {
            await _coreChannel.Writer.WriteAsync(variant);
            await _alleleFreqChannel.Writer.WriteAsync(variant);
            await _idChannel.Writer.WriteAsync(variant);
            await _clinicalChannel.Writer.WriteAsync(variant);
        }
        
        public void Complete()
        {
            _coreChannel.Writer.Complete();
            _alleleFreqChannel.Writer.Complete();
            _coreTask.Wait();
            _alleleFreqTask.Wait();
        }

        private async void CoreAnnotation()
        {
            while (await _coreChannel.Reader.WaitToReadAsync())
            {
                while (_coreChannel.Reader.TryRead(out var variant))
                {
                    CoreAnnotationProvider.Annotate(variant);
                    //await _saChannel.Writer.WriteAsync(variant);
                }
            }
            //_saChannel.Writer.Complete();
        }
        
        private async void AddAlleleFreq()
        {
            while (await _alleleFreqChannel.Reader.WaitToReadAsync())
            {
                while (_alleleFreqChannel.Reader.TryRead(out var variant))
                {
                    AlleleFreqProvider.Annotate(variant);
                }
            }
            
        }
        private async void AddIds()
        {
            while (await _idChannel.Reader.WaitToReadAsync())
            {
                while (_idChannel.Reader.TryRead(out var variant))
                {
                    VariantIdProvider.Annotate(variant);
                }
            }
            
        }
        
        private async void AddClinical()
        {
            while (await _clinicalChannel.Reader.WaitToReadAsync())
            {
                while (_clinicalChannel.Reader.TryRead(out var variant))
                {
                    ClinicalAnnotationProvider.Annotate(variant);
                }
            }
            
        }

    }
}