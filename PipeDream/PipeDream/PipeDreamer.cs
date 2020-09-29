using System;
using BenchmarkDotNet.Running;

namespace PipeDream
{
    public class PipeDreamer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dreaming for the optimal pipeline!");
            BenchmarkSwitcher.FromAssembly(typeof(PipeDreamer).Assembly).Run(args);
        }

        
    }
}