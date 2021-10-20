// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace CpuPerformanceTester
{
    public class Program
    {
        public static void Main()
        {
            for(var n = 1; n < 10; n++)
                RunDiskTest(n);

            for(var n = 1; n < 50; n++)
                RunCpuTest(n);
        }

        static void RunDiskTest(int threadCount)
        {
            var sw = Stopwatch.StartNew();

            var threads = Enumerable.Range(0, threadCount)
                .Select(_ => new Thread(WriteAndReadFile))
                .ToArray();

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();

            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine($"Disk with {threadCount} threads took {elapsed}ms ({elapsed/threadCount}ms per thread)");
        }

        private static void WriteAndReadFile()
        {
            var filename = "TestFile" + Guid.NewGuid();
            var buffer = new byte[200000];

            var rnd = new Random();
            rnd.NextBytes(buffer);

            using (var f = File.OpenWrite(filename))
            {
                var itterations = 1_000_000_000 / buffer.Length;
                for (var x = 0; x < itterations; x++)
                {
                    f.Write(buffer, 0, buffer.Length);
                }
            }
            using (var f = File.OpenRead(filename))
                while (f.Read(buffer, 0, buffer.Length) > 0)
                {
                }
        }

        public static void RunCpuTest(int threadCount)
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
            Console.WriteLine($"CPU with {threadCount} threads took {elapsed}ms ({elapsed/threadCount}ms per thread)");
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