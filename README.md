AdvAlgo2014
===========

Assignments for Advanced Algorithms 2014 course at Todai.

[Assignment 1](http://www-imai.is.s.u-tokyo.ac.jp/~kawamura/teaching/advalgo/AA14-part1-assignments.pdf) - Solovay-Strassen primality test:

1. Source code is [here](https://github.com/gladnik/AdvAlgo2014/blob/master/PrimalityTesting_SolovayStrassen/PrimalityTesting_SolovayStrassen/SolovayStrassen.cs).

 ```C#
public static bool Test(BigInteger n) 
{
    if (n == 2) return true;
    else if (n < 2 || n.IsEven) return false;
    BigInteger a = GetRandomInRange(2, n - 1);
    if (BigInteger.GreatestCommonDivisor(a, n) != 1) return false;
    else return BigInteger.ModPow(a, n >> 1, n) == BigInteger.Remainder(JacobiSymbol(a, n) + n, n);
}
    ```
2. Data for the diagram in the report is [here](https://www.dropbox.com/s/uj66r5lfqepb4ps/AdvAlgo2014%20-%20Data.xlsx).
