// Author : Vandewynckel Julien
// Creation date : 13/03/2015
// Last modified date : 05/05/2015

namespace CSharpMetal.Util
{
    public static class MetalMath
    {
        public static int Factorial(int n)
        {
            if (n == 1)
            {
                return 1;
            }
            return n*Factorial(n - 1);
        }

        /// <summary>
        ///     Compute the
        ///     (n)
        ///     (k)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int BinomialCoeff(int k, int n)
        {
            return Factorial(n)/(Factorial(k)*Factorial(n - k));
        }
    }
}