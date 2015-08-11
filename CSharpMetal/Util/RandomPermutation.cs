// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;

namespace CSharpMetal.Util
{
    public static class RandomPermutation
    {
        public static void execute(int[] perm, int size)
        {
            // <pex>
            if (perm == null)
            {
                throw new ArgumentNullException("perm");
            }
            if (perm.Length < 2)
            {
                throw new ArgumentException("perm.Length < 2", "perm");
            }
            // </pex>
            var index = new int[size];
            var flag = new bool[size];

            for (var n = 0; n < size; n++)
            {
                index[n] = n;
                flag[n] = true;
            }

            var num = 0;
            while (num < size)
            {
                int start = PseudoRandom.Instance().Next(0, size - 1);
                //int start = int(size*nd_uni(&rnd_uni_init));
                while (true)
                {
                    if (flag[start])
                    {
                        perm[num] = index[start];
                        flag[start] = false;
                        num++;
                        break;
                    }
                    if (start == (size - 1))
                    {
                        start = 0;
                    }
                    else
                    {
                        start++;
                    }
                }
            }
            // while
        }
    } // RandomPermutation
}