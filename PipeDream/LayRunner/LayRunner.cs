using System;
using PipeDream;

namespace LayRunner
{
    public static class LayRunner
    {
        private const int DefaultIterationCount = 1;
        static void Main(string[] args)
        {
            var iterationCount = args.Length > 0 ? int.Parse(args[0]) : DefaultIterationCount; 
            Console.WriteLine("Timing pipelines!");
            var pipelines = new Pipelines();
            
             var batchTimeSpan = GetBatchRunTime(pipelines, iterationCount);
             Console.WriteLine($"Batch run time (seconds) per iteration:"+ batchTimeSpan.TotalSeconds/DefaultIterationCount);
            
             var parallelTimeSpan = GetParallelRunTime(pipelines, iterationCount);
             Console.WriteLine($"parallel run time (seconds) per iteration:"+ parallelTimeSpan.TotalSeconds/DefaultIterationCount);
            
             var serialTimeSpan = GetSerialRunTime(pipelines, iterationCount);
            Console.WriteLine($"serial run time (seconds) per iteration:"+ serialTimeSpan.TotalSeconds/DefaultIterationCount);
            
            var concurrentTimeSpan = GetConcurrentRunTime(pipelines, iterationCount);
            Console.WriteLine("Concurrent queue run time (seconds) per iteration:"+ concurrentTimeSpan.TotalSeconds/iterationCount);
            
            var channelTimeSpan = GetChannelRunTime(pipelines, iterationCount);
            Console.WriteLine("Channel annotator run time (seconds) per iteration:"+ channelTimeSpan.TotalSeconds/iterationCount);
        }

        private static TimeSpan GetBatchRunTime(Pipelines pipelines, int iterationCount)
        {

            Console.WriteLine("running in batches");
            var tick = DateTime.Now;

            for (int i = 0; i < iterationCount; i++)
            {
                pipelines.BatchAnnotation();
                //Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
        
        private static TimeSpan GetParallelRunTime(Pipelines pipelines, int iterationCount)
        {
            Console.WriteLine("running in parallel");
            var tick = DateTime.Now;

            for (int i = 0; i < iterationCount; i++)
            {
                pipelines.ParallelAnnotation();
                //Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
        
        private static TimeSpan GetConcurrentRunTime(Pipelines pipelines, int iterationCount)
        {
            Console.WriteLine("running concurrent queue");
            var tick = DateTime.Now;

            for (int i = 0; i < iterationCount; i++)
            {
                pipelines.ConcurrentAnnotation();
                //Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
        
        private static TimeSpan GetChannelRunTime(Pipelines pipelines, int iterationCount)
        {
            Console.WriteLine("running channel annotator");
            var tick = DateTime.Now;

            for (int i = 0; i < iterationCount; i++)
            {
                pipelines.ChannelAnnotation();
                //Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var timeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return timeSpan;
        }
        private static TimeSpan GetSerialRunTime(Pipelines pipelines, int iterationCount)
        {
            Console.WriteLine("running in serial");
            var tick = DateTime.Now;

            for (int i = 0; i < iterationCount; i++)
            {
                pipelines.SerialAnnotation();
                //Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
    }
}