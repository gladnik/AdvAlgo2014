using System;
using System.Numerics;

namespace PrimalityTesting_SolovayStrassen
{
    class SolovayStrassen
    {
        private static Random random = new Random();

        private static BigInteger GetRandomInRange(BigInteger min, BigInteger max) // min <= result <= max
        {
            BigInteger result;
            byte[] bytes = max.ToByteArray();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                result = new BigInteger(bytes);
            }
            while (result > max || result < min);

            return result;
        }

        private static int JacobiSymbol(BigInteger a, BigInteger n) //Outputs only 1 or -1 as we check GCD separetly
        {
            bool flag = true;
            int shift;
            BigInteger remainder, temp;
            do
            {
                shift = 0;
                while ((a >> shift).IsEven)
                {
                    shift++;
                }
                a >>= shift;
                if ((shift & 1) == 1) //odd
                {
                    remainder = BigInteger.Remainder(n, 8);
                    if (remainder == 3 || remainder == 5)
                    {
                        flag = !flag;
                    }
                }
                if (BigInteger.Remainder(a, 4) == 3 && BigInteger.Remainder(n, 4) == 3)
                {
                    flag = !flag;
                }

                temp = a;
                a = BigInteger.Remainder(n, temp);
                n = temp;
            }
            while (a > 0);

            return flag ? 1 : -1;
        }

        public static bool Test(BigInteger n)
        {
            if (n == 2) return true;
            else if (n < 2 || n.IsEven) return false;
            BigInteger a = GetRandomInRange(2, n - 1);
            if (BigInteger.GreatestCommonDivisor(a, n) != 1) return false;
            else return BigInteger.ModPow(a, n >> 1, n) == BigInteger.Remainder(JacobiSymbol(a, n) + n, n);
        }
    }
}
