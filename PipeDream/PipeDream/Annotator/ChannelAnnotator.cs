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
        private readonly Channel<AnnotatedVariant> _variantChannel;
        private readonly Channel<AnnotatedVariant> _coreChannel;
        private readonly Channel<AnnotatedVariant> _saChannel;
        private readonly StreamWriter _writer;

        public ChannelAnnotator(StreamWriter writer, int size = DefaultSize)
        {
            _writer = writer;
            _variantChannel = Channel.CreateBounded<AnnotatedVariant>(DefaultSize);
            _coreChannel = Channel.CreateBounded<AnnotatedVariant>(DefaultSize);
            _saChannel = Channel.CreateBounded<AnnotatedVariant>(DefaultSize);
        }

        public async void Submit(AnnotatedVariant variant)
        {
            await _variantChannel.Writer.WriteAsync(variant);
        }

        public void Run()
        {
            var coreThread = new Thread(CoreAnnotation);
            var saThread = new Thread(SaAnnotation);
            var writerThread = new Thread(WriteOut);
            coreThread.Start();
            saThread.Start();
            writerThread.Start();
            
            
        }

        public void Complete()
        {
            _variantChannel.Writer.Complete();
        }

        private async void CoreAnnotation()
        {
            while (await _variantChannel.Reader.WaitToReadAsync())
            {
                while (_variantChannel.Reader.TryRead(out var variant))
                {
                    CoreAnnotationProvider.Annotate(variant);
                    await _coreChannel.Writer.WriteAsync(variant);
                }
            }
            //once all variants from the variant channel has been annotated and written, mark writer complete
            _coreChannel.Writer.Complete();
        }
        
        private async void SaAnnotation()
        {
            while (await _coreChannel.Reader.WaitToReadAsync())
            {
                while (_coreChannel.Reader.TryRead(out var variant))
                {
                    SuppAnnotationProvider.Annotate(variant);
                    await _saChannel.Writer.WriteAsync(variant);
                }
            }
            //once all variants from the core channel has been annotated and written, mark writer complete
            _saChannel.Writer.Complete();
        }

        private async void WriteOut()
        {
            while (await _saChannel.Reader.WaitToReadAsync())
            {
                while (_saChannel.Reader.TryRead(out var variant))
                {
                    _writer.Write(Utf8Json.JsonSerializer.Serialize(variant));
                }
            }
        }

    }
}