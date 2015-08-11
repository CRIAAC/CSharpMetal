// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;

namespace CSharpMetal.Util
{
    public static class MinFastSort
    {
        public static void execute(double[] x, int[] idx, int n, int m)
        {
            // <pex>
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (x.Length == 0)
            {
                throw new ArgumentException("x.Length == 0", "x");
            }
            // </pex>
            for (var i = 0; i < m; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (x[i] > x[j])
                    {
                        double temp = x[i];
                        x[i] = x[j];
                        x[j] = temp;
                        int id = idx[i];
                        idx[i] = idx[j];
                        idx[j] = id;
                    }
                    // if
                }
            }
            // for
        }
    }
}