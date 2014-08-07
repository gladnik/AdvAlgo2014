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
2. Data for the diagram in the report is [here](https://www.dropbox.com/s/1jjqtz272s13xul/AdvAlgo2014%20-%20Assignment%201%20-%20Data.xlsx). 

[Assignment 3](http://www.hgc.jp/~tshibuya/classes/aa2014_4communication.pdf) - Finding the longest tandem repeat in a string with Main-Lorentz 1982 algorithm **O(n*log(n))**:
 
1. Source code is [here](https://github.com/gladnik/AdvAlgo2014/blob/master/LongestTandemRepeat/LongestTandemRepeat/Program.cs).
 
 ```C#
 private void FindAllTandems(String str, int shift = 0)
 {
    if (str.Length < 2) return;
    
    String u = str.Substring(0, str.Length / 2);
    String v = str.Substring(str.Length / 2);
    String uReverse = Reverse(u);
    String vReverse = Reverse(v);
    
    List<int> z1 = ZFunction(uReverse);
    List<int> z2 = ZFunction(v + '#' + u);
    List<int> z3 = ZFunction(uReverse + '#' + vReverse);
    List<int> z4 = ZFunction(v);
    
    for (int cntr = 0; cntr < str.Length; cntr++)
    {
        int l, k1, k2;
        if (cntr < u.Length)
        {
            l = u.Length - cntr;
            k1 = GetZ(z1, u.Length - cntr);
            k2 = GetZ(z2, v.Length + 1 + cntr);
        }
        else
        {
            l = cntr - u.Length + 1;
            k1 = GetZ(z3, u.Length + 1 + v.Length - 1 - (cntr - u.Length));
            k2 = GetZ(z4, (cntr - u.Length) + 1);
        }
        if (k1 + k2 >= l)
        {
            this.tandemsLocationZipped.Add(new TandemsLocationZipped(l, cntr < u.Length, cntr + shift, k1, k2));
        }
    }
    FindAllTandems(u, shift);
    FindAllTandems(v, shift + u.Length);
 }
 ```
    
2. Data for the diagram in the report is [here](https://www.dropbox.com/s/do0dof8jbjmmuxo/AdvAlgo2014%20-%20Assignment%203%20-%20Data.xlsx).
