using System;
using System.Collections.Generic;

namespace LongestTandemRepeat
{
    class Program
    {
        static void Main()
        {
            //Some string to demonstrate the outcome of the program
            String s = "abcabcdefdefaabbb"; //Tandem repeats are: aa, bb, bb, abcabc, defdef
            Console.WriteLine("Input string: " + s);
            Console.WriteLine("Tandem repeats are: aa, bb, bb, abcabc, defdef\n");

            //Trivial methods to find the first longest tandem repeat in a string
            Console.WriteLine("Shift and XOR result: " + Trivial.ShiftAndXor(s));
            Console.WriteLine("Cyclical search result: " + Trivial.Cycles(s));

            //Main-Lorentz for the first longest tandem repeat in a string
            MainLorentz82 ml82Longest = new MainLorentz82(s);
            ml82Longest.FindAllTandems();
            ml82Longest.UnzipLongestTandems();
            Console.WriteLine("Main-Lorentz algorithm result: " + ml82Longest.GetLongestTandem() + "\n");

            //Main-Lorentz for all longest tandem repeats in a string
            Console.WriteLine("Main-Lorentz algorithm - all longest tandems:");
            MainLorentz82 ml82AllLongest = new MainLorentz82(s);
            ml82AllLongest.FindAllTandems();
            ml82AllLongest.UnzipLongestTandems();
            ml82AllLongest.OutputTandems();
            Console.WriteLine();

            //Main-Lorentz for all tandem repeats in a string
            Console.WriteLine("Main-Lorentz algorithm - all tandems:");
            MainLorentz82 ml82All = new MainLorentz82(s);
            ml82All.FindAllTandems();
            ml82All.UnzipAllTandems();
            ml82All.OutputTandems();
            Console.WriteLine();

            //Tests to check correctness of the result
            //All test are for less than 30 000 characters length strings
            Testing.FibonacciTest(); //Fibonacci strings: str_next = str_lastlast + str_last, str_0 = "a", str_1 = "b"
            Testing.RepetitionTestA(); //Repetitions: every iteration adds 500 'a' characters to the string
            Testing.RandomTest(4); //Random test 4: every iteration adds 500 characters, each randomly chosen from {'A','B','C','D'}
            Testing.RandomTest(26); //Random test 26: every iteration adds 500 characters, each randomly chosen from {'A','B',...,'Z'} 

            Console.ReadKey();
        }

        /// <summary> Tests to check correctness of the result.</summary>
        /// <remarks> All test are for less than 30 000 characters length strings</remarks>
        class Testing
        {
            /// <summary> Fibonacci strings: str_next = str_lastlast + str_last, str_0 = "a", str_1 = "b".</summary>
            public static void FibonacciTest()
            {
                Console.WriteLine("Fibonacci test started:");
                String strLastLast = "a";
                String strLast = "b";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while (sb.Length < 30000)
                {
                    sb.Append(strLastLast + strLast);
                    strLastLast = strLast;
                    strLast = sb.ToString();
                    CompareResults(sb.ToString());
                }
                Console.WriteLine("Fibonacci test ended.\n");
            }
            /// <summary> Repetitions: every iteration adds 500 'a' characters to the string.</summary>
            public static void RepetitionTestA()
            {
                Console.WriteLine("Repetition test started:");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while (sb.Length < 30000)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        sb.Append("aaaaaaaaaa");
                    }
                    CompareResults(sb.ToString());
                }
                Console.WriteLine("Repetition test ended.\n");
            }
            /// <summary> Random test N: every iteration adds 500 characters, each randomly chosen from the alphabet of size N.</summary>
            public static void RandomTest(int alphabetSize)
            {
                Console.WriteLine("Random test started for " + alphabetSize + "-characters alphabet:");
                Random rand = new Random();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while (sb.Length < 30000)
                {
                    for (int i = 0; i < 500; i++)
                    {
                        sb.Append(Convert.ToChar(rand.Next(alphabetSize) + 65));
                    }
                    CompareResults(sb.ToString());
                }
                Console.WriteLine("Random test ended.\n");
            }
            /// <summary> Runs all three algorithms on a given string and checks they results are equal. Outputs time of execution for each algorithm.</summary>
            public static void CompareResults(String str)
            {
                DateTime start = DateTime.Now, firstEnd, secondEnd, thirdEnd;

                String shiftAndXor = Trivial.ShiftAndXor(str);
                firstEnd = DateTime.Now;

                String cycles = Trivial.Cycles(str);
                secondEnd = DateTime.Now;

                MainLorentz82 ml82 = new MainLorentz82(str);
                ml82.FindAllTandems();
                ml82.UnzipLongestTandems();
                String ml = ml82.GetLongestTandem();
                thirdEnd = DateTime.Now;

                bool first = shiftAndXor == cycles;
                bool second = shiftAndXor == ml;
                bool third = cycles == ml;

                Console.Write(str.Length + "\t" + (firstEnd - start).TotalMilliseconds + "\t\t" + (secondEnd - firstEnd).TotalMilliseconds + "\t\t" + (thirdEnd - secondEnd).TotalMilliseconds + "\t");
                if (first && second && third)
                {
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.WriteLine(first + " " + second + " " + third + " " + str);
                    Console.ReadKey();
                }
            }
        }

        /// <summary> Two simple algorithms for finding the first longest tandem repeat.
        /// Both have complexity O(n^2).</summary>
        class Trivial
        {
            /// <summary> On each iteration shifts the string for 1 character to the right and XORs it with original string.
            /// Tandem repeat is found if on i-th step the result of XOR contains i sequential zeroes.</summary>
            public static String ShiftAndXor(String str)
            {
                int longestTandemStart = 0;
                int longestTandemLength = 0;
                for (int i = 1; i <= str.Length / 2; i++)
                {
                    int equalsCounter = 0;
                    for (int j = 0; j < str.Length - i; j++)
                    {
                        if (str[i + j] == str[j])
                        {
                            equalsCounter++;
                            if (equalsCounter == i)
                            {
                                longestTandemStart = j - i + 1;
                                longestTandemLength = i;
                                break;
                            }
                        }
                        else
                        {
                            equalsCounter = 0;
                        }
                    }
                }
                return str.Substring(longestTandemStart, 2 * longestTandemLength);
            }
            /// <summary> For each character in a string tries to find a pair. If the pair is found, checks whether it is a tandem repeat.
            /// </summary>
            public static String Cycles(String str)
            {
                int longestTandemStart = 0;
                int longestTandemLength = 0;
                for (int i = 0; i < str.Length - 1; i++)
                {
                    for (int j = i + 1; j <= (str.Length + i) / 2; j++)
                    {
                        if (j - i > longestTandemLength)
                        {
                            int equalsCounter = 0;
                            for (int k = 0; k < j - i; k++)
                            {
                                if (str[i + k] == str[j + k])
                                {
                                    equalsCounter++;
                                    if (equalsCounter == j - i)
                                    {
                                        longestTandemStart = i;
                                        longestTandemLength = j - i;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                return str.Substring(longestTandemStart, 2 * longestTandemLength);
            }
        }

        /// <summary> This class implements Main-Lorentz (1982) algorithm for finding tandem repeats in a string of length n with O(n*log(n)) operations.
        /// <remarks> This implementaion uses '#' as a special charcter, which should not be present in the input string.</remarks>
        /// </summary>
        class MainLorentz82
        {
            public String initialString;
            /// <summary> Explicit representation of tandem repeats: starting position and length.</summary>
            public struct TandemsLocation
            {
                public int startPos;
                public int length;
                public TandemsLocation(int pos, int len)
                {
                    startPos = pos;
                    length = len;
                }
            }
            /// <summary> Compressed representation of a group of tandem repeats with center cntr, 
            /// such that all the following three inequalities are satisfied:
            /// l1 is less or equal than k1
            /// l2 is less or equal than k2
            /// l1 + l2 = length
            /// </summary>
            public struct TandemsLocationZipped
            {
                public int length;
                public bool left;
                public int cntr;
                public int k1;
                public int k2;
                public TandemsLocationZipped(int len, bool isLeft, int pos, int k1Len, int k2Len)
                {
                    length = len;
                    left = isLeft;
                    cntr = pos;
                    k1 = k1Len;
                    k2 = k2Len;
                }
            }
            public List<TandemsLocation> tandemsLocation;
            public List<TandemsLocationZipped> tandemsLocationZipped;
            public MainLorentz82(String inputString) //Class constructor
            {
                initialString = inputString;
                tandemsLocation = new List<TandemsLocation>();
                tandemsLocationZipped = new List<TandemsLocationZipped>();
            }

            /// <summary> Reverses a string.
            /// <returns> Returns a string with reversed order of characters.</returns>
            /// </summary>
            private static String Reverse(String str)
            {
                if (str == null) return null;

                char[] array = str.ToCharArray();
                Array.Reverse(array);
                return new String(array);
            }
            /// <summary> Computes Z-function of a string. http://codeforces.com/blog/entry/3107.
            /// <returns> Returns a list of integers, where i-th element equals the length of the longest common prefix of a string and its i-th suffix.</returns>
            /// <remarks> z[0] = 0.</remarks>
            /// <seealso cref="http://e-maxx.ru/algo/z_function"/>
            /// </summary>
            private static List<int> ZFunction(String str)
            {
                List<int> z = new List<int>();
                for (int i = 0; i < str.Length; i++)
                {
                    z.Add(0);
                }
                for (int i = 1, l = 0, r = 0; i < str.Length; i++)
                {
                    if (i < r)
                    {
                        z[i] = Math.Min(r - i, z[i - l]);
                    }
                    while (i + z[i] < str.Length && str[z[i]] == str[i + z[i]])
                    {
                        z[i]++;
                    }
                    if (i + z[i] > r)
                    {
                        l = i;
                        r = i + z[i];
                    }
                }
                return z;
            }
            /// <summary> Returns z[i] if it exists.</summary>
            private static int GetZ(List<int> z, int i)
            {
                return (0 <= i && i < z.Count) ? z[i] : 0;
            }
            /// <summary> Main-Lorentz algorithm for finding all tandem repeats in a string.
            /// The algorithm is based on divide-and-conquer technique. 
            /// It unilizes O(n) operations to find tandem repeats which overlap the center of the current string. 
            /// Then process repeats for each half of the string.
            /// So the overall complexity is O(n*log(n)).
            /// </summary>
            /// <remarks> This implementaion uses '#' as a special charcter, which should not be present in the input string.</remarks>
            /// <seealso cref="http://digitool.library.colostate.edu///exlibris/dtl/d3_1/apache_media/L2V4bGlicmlzL2R0bC9kM18xL2FwYWNoZV9tZWRpYS8xNjY2ODE=.pdf"/>
            public void FindAllTandems()
            {
                FindAllTandems(initialString);
            }
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

            /// <summary> Converts a condensed representation of a group of tandem repeats into explicit form.</summary>
            public void UnzipTandem(TandemsLocationZipped tandemZipped)
            {
                for (int l1 = 1; l1 <= tandemZipped.length; ++l1)
                {
                    if (tandemZipped.left && l1 == tandemZipped.length)
                    {
                        break;
                    }
                    if (l1 <= tandemZipped.k1 && tandemZipped.length - l1 <= tandemZipped.k2)
                    {
                        int pos;
                        if (tandemZipped.left)
                        {
                            pos = tandemZipped.cntr - l1;
                        }
                        else
                        {
                            pos = tandemZipped.cntr - l1 - tandemZipped.length + 1;
                        }
                        this.tandemsLocation.Add(new TandemsLocation(pos, 2 * tandemZipped.length));
                    }
                }

            }
            /// <summary> Converts condensed representation of all tandem repeats into explicit form.</summary>
            public void UnzipAllTandems()
            {
                foreach (TandemsLocationZipped tandemZipped in this.tandemsLocationZipped)
                {
                    this.UnzipTandem(tandemZipped);
                }
            }
            /// <summary> Converts condensed representation of all the longest tandem repeats into explicit form.</summary>
            public void UnzipLongestTandems()
            {
                int maxLength = 0;
                List<int> longestTandems = new List<int>();
                for (int i = 0; i < this.tandemsLocationZipped.Count; i++)
                {
                    if (this.tandemsLocationZipped[i].length > maxLength)
                    {
                        longestTandems.Clear();
                        longestTandems.Add(i);
                        maxLength = this.tandemsLocationZipped[i].length;
                    }
                    else if (this.tandemsLocationZipped[i].length == maxLength)
                    {
                        longestTandems.Add(i);
                    }
                }
                foreach (int i in longestTandems)
                {
                    this.UnzipTandem(this.tandemsLocationZipped[i]);

                }
            }
            /// <summary> Prints out all explicitly represented tandems.</summary>
            public void OutputTandems()
            {
                foreach (TandemsLocation tandem in this.tandemsLocation)
                {
                    Console.Write("Tandem of length " + tandem.length + " at [" + tandem.startPos + ".." + (tandem.startPos + tandem.length - 1) + "] ");
                    Console.WriteLine(this.initialString.Substring(tandem.startPos, tandem.length));
                }
            }

            /// <summary> Returns a string with the first longest tandem repeat in a string.</summary>
            public String GetLongestTandem() //For testing
            {
                if (tandemsLocation.Count > 0)
                {
                    int minPos = this.initialString.Length;
                    int minPosTandem = 0;
                    for (int i = 0; i < this.tandemsLocation.Count; i++)
                    {
                        if (this.tandemsLocation[i].startPos < minPos)
                        {
                            minPos = this.tandemsLocation[i].startPos;
                            minPosTandem = i;
                        }
                    }
                    return this.initialString.Substring(tandemsLocation[minPosTandem].startPos, tandemsLocation[0].length);
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
