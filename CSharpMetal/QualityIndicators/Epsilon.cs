// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;

namespace CSharpMetal.QualityIndicators
{
    internal class Epsilon
    {
        /* stores the number of objectives */
        private int _dim;
        /* method_ = 0 means apply additive epsilon and method_ = 1 means multiplicative
         * epsilon. This code always apply additive epsilon
         */
        private int _method;
        /* obj_[i]=0 means objective i is to be minimized. This code always assume the minimization of all the objectives
         */
        private int[] _obj; /* obj_[i] = 0 means objective i is to be minimized */
        /**
 * Returns the epsilon indicator.
 * @param b True Pareto front
 * @param a Solution front
 * @return the value of the epsilon indicator
 */

        public double Compute(double[][] b, double[][] a, int dim)
        {
            int i;
            double epsJ = 0.0, epsK = 0.0;

            _dim = dim;
            set_params();

            double eps = _method == 0 ? double.MinValue : 0;

            for (i = 0; i < a.Length; i++)
            {
                int j;
                for (j = 0; j < b.Length; j++)
                {
                    int k;
                    for (k = 0; k < _dim; k++)
                    {
                        double epsTemp;
                        switch (_method)
                        {
                            case 0:
                                if (_obj[k] == 0)
                                {
                                    epsTemp = b[j][k] - a[i][k];
                                }
                                //eps_temp = b[j * dim_ + k] - a[i * dim_ + k];
                                else
                                {
                                    epsTemp = a[i][k] - b[j][k];
                                }
                                //eps_temp = a[i * dim_ + k] - b[j * dim_ + k];
                                break;
                            default:
                                if ((a[i][k] < 0 && b[j][k] > 0) ||
                                    (a[i][k] > 0 && b[j][k] < 0) ||
                                    (Math.Abs(a[i][k]) < Double.Epsilon || Math.Abs(b[j][k]) < Double.Epsilon))
                                {
                                    //if ( (a[i * dim_ + k] < 0 && b[j * dim_ + k] > 0) ||
                                    //     (a[i * dim_ + k] > 0 && b[j * dim_ + k] < 0) ||
                                    //     (a[i * dim_ + k] == 0 || b[j * dim_ + k] == 0)) {
                                    throw new Exception("error in data file");
                                }
                                if (_obj[k] == 0)
                                {
                                    epsTemp = b[j][k]/a[i][k];
                                }
                                //eps_temp = b[j * dim_ + k] / a[i * dim_ + k];
                                else
                                {
                                    epsTemp = a[i][k]/b[j][k];
                                }
                                //eps_temp = a[i * dim_ + k] / b[j * dim_ + k];
                                break;
                        }
                        if (k == 0)
                        {
                            epsK = epsTemp;
                        }
                        else if (epsK < epsTemp)
                        {
                            epsK = epsTemp;
                        }
                    }
                    if (j == 0)
                    {
                        epsJ = epsK;
                    }
                    else if (epsJ > epsK)
                    {
                        epsJ = epsK;
                    }
                }
                if (i == 0)
                {
                    eps = epsJ;
                }
                else if (eps < epsJ)
                {
                    eps = epsJ;
                }
            }
            return eps;
        } // epsilon
        /**
         * Established the params by default
         */

        private void set_params()
        {
            int i;
            _obj = new int[_dim];
            for (i = 0; i < _dim; i++)
            {
                _obj[i] = 0;
            }
            _method = 0;
        }
    }
}