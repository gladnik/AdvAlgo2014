using System;
using System.Numerics;

namespace PrimalityTesting_SolovayStrassen
{
    class Program
    {
        static void Main()
        {
            DateTime start, end;
            start = DateTime.Now;
            Console.WriteLine(SolovayStrassen.Test(BigInteger.Pow(2, 9941) - 1));
            end = DateTime.Now;
            Console.WriteLine((end - start).ToString());
            Console.ReadKey();
        }
    }
}