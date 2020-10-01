using System;
using PipeDream;

namespace LayRunner
{
    public class LayRunner
    {
        private const int IterationCount = 50;
        static void Main(string[] args)
        {
            Console.WriteLine("Timing pipelines!");
            var pipelines = new Pipelines();
            
            var batchTimeSpan = GetBatchRunTime(pipelines);
            Console.WriteLine($"Batch run time (seconds) per iteration:"+ batchTimeSpan.TotalSeconds/IterationCount);
            
            var parallelTimeSpan = GetParallelRunTime(pipelines);
            Console.WriteLine($"parallel run time (seconds) per iteration:"+ parallelTimeSpan.TotalSeconds/IterationCount);
            
            var serialTimeSpan = GetSerialRunTime(pipelines);
            Console.WriteLine($"serial run time (seconds) per iteration:"+ serialTimeSpan.TotalSeconds/IterationCount);
        }

        private static TimeSpan GetBatchRunTime(Pipelines pipelines)
        {

            Console.WriteLine("running in batches");
            var tick = DateTime.Now;

            for (int i = 0; i < IterationCount; i++)
            {
                pipelines.BatchAnnotation();
                Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
        
        private static TimeSpan GetParallelRunTime(Pipelines pipelines)
        {
            Console.WriteLine("running in parallel");
            var tick = DateTime.Now;

            for (int i = 0; i < IterationCount; i++)
            {
                pipelines.ParallelAnnotation();
                Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
        private static TimeSpan GetSerialRunTime(Pipelines pipelines)
        {
            Console.WriteLine("running in serial");
            var tick = DateTime.Now;

            for (int i = 0; i < IterationCount; i++)
            {
                pipelines.SerialAnnotation();
                Console.WriteLine($"completed run {i + 1}");
            }

            var tock = DateTime.Now;

            var batchTimeSpan = new TimeSpan(tock.Ticks - tick.Ticks);
            return batchTimeSpan;
        }
    }
}