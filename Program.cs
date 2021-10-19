// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace CpuPerformanceTester
{
    public class Program
    {
        public static void Main()
        {
            for(var n = 1; n < 20; n++)
                Run(n);
        }

        public static void Run(int threadCount)
        {
            var sw = Stopwatch.StartNew();

            var threads = Enumerable.Range(0, threadCount)
                .Select(_ => new Thread(() => FindPrimeNumber(200000)))
                .ToArray();

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();

            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine($"{threadCount} threads took {elapsed}ms ({elapsed/threadCount}ms per thread)");
        }

        static long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                bool prime = true;
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = false;
                        break;
                    }
                    b++;
                }
                if (prime)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }
    }
}