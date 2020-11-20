using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using PipeDream.VariantAnnotation;

namespace PipeDream.Annotator
{
    public class ChannelAnnotator
    {
        public const int DefaultSize = 50;
        private readonly Channel<AnnotatedVariant> _coreChannel;
        private readonly Channel<AnnotatedVariant> _saChannel;
        //private readonly StreamWriter _writer;
        private bool _leaveOpen;
        private Thread _coreThread, _saThread;

        public ChannelAnnotator( int size = DefaultSize, bool leaveOpen = false)
        {
            //_writer = writer;
            _leaveOpen = leaveOpen;
            _coreChannel = Channel.CreateBounded<AnnotatedVariant>(DefaultSize);
            _saChannel = Channel.CreateBounded<AnnotatedVariant>(DefaultSize);
            
            _coreThread = new Thread(CoreAnnotation);
            _saThread = new Thread(SaAnnotation);
            
            _coreThread.Start();
            _saThread.Start();
            
        }

        public async ValueTask Submit(AnnotatedVariant variant)
        {
            await _coreChannel.Writer.WriteAsync(variant);
            await _saChannel.Writer.WriteAsync(variant);
        }
        
        public void Complete()
        {
            _coreChannel.Writer.Complete();
            _saChannel.Writer.Complete();
            _coreThread.Join();
            _saThread.Join();
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
        
        private async void SaAnnotation()
        {
            while (await _saChannel.Reader.WaitToReadAsync())
            {
                while (_saChannel.Reader.TryRead(out var variant))
                {
                    SuppAnnotationProvider.Annotate(variant);
                }
            }
            
        }

    }
}