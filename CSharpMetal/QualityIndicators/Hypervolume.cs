// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.QualityIndicators.Util;

namespace CSharpMetal.QualityIndicators
{
    public class Hypervolume
    {
        private bool Dominates(double[] point1, double[] point2, int noObjectives)
        {
            int i;

            var betterInAnyObjective = 0;
            for (i = 0; i < noObjectives && point1[i] >= point2[i]; i++)
            {
                if (point1[i] > point2[i])
                {
                    betterInAnyObjective = 1;
                }
            }

            return ((i >= noObjectives) && (betterInAnyObjective > 0));
        } //Dominates

        private void Swap(double[][] front, int i, int j)
        {
            double[] temp = front[i];
            front[i] = front[j];
            front[j] = temp;
        } // Swap 
        /* all nondominated points regarding the first 'noObjectives' dimensions
  are collected; the points referenced by 'front[0..noPoints-1]' are
  considered; 'front' is resorted, such that 'front[0..n-1]' contains
  the nondominated points; n is returned */

        private int FilterNondominatedSet(double[][] front, int noPoints, int noObjectives)
        {
            int i, j;
            int n;

            n = noPoints;
            i = 0;
            while (i < n)
            {
                j = i + 1;
                while (j < n)
                {
                    if (Dominates(front[i], front[j], noObjectives))
                    {
                        /* remove point 'j' */
                        n--;
                        Swap(front, j, n);
                    }
                    else if (Dominates(front[j], front[i], noObjectives))
                    {
                        /* remove point 'i'; ensure that the point copied to index 'i'
	   is considered in the next outer loop (thus, decrement i) */
                        n--;
                        Swap(front, i, n);
                        i--;
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
                i++;
            }
            return n;
        }

        /* calculate next value regarding dimension 'objective'; consider
     points referenced in 'front[0..noPoints-1]' */

        private double SurfaceUnchangedTo(double[][] front, int noPoints, int objective)
        {
            int i;

            if (noPoints < 1)
            {
                throw new Exception("The number of point is lesser than 1");
            }

            double minValue = front[0][objective];
            for (i = 1; i < noPoints; i++)
            {
                double value = front[i][objective];
                if (value < minValue)
                {
                    minValue = value;
                }
            }
            return minValue;
        }

        /* remove all points which have a value <= 'threshold' regarding the
     dimension 'objective'; the points referenced by
     'front[0..noPoints-1]' are considered; 'front' is resorted, such that
     'front[0..n-1]' contains the remaining points; 'n' is returned */

        private int ReduceNondominatedSet(double[][] front, int noPoints, int objective,
                                          double threshold)
        {
            int i;

            int n = noPoints;
            for (i = 0; i < n; i++)
            {
                if (front[i][objective] <= threshold)
                {
                    n--;
                    Swap(front, i, n);
                }
            }

            return n;
        } // ReduceNondominatedSet

        public double CalculateHypervolume(double[][] front, int noPoints, int noObjectives)
        {
            double volume = 0;
            double distance = 0;
            int n = noPoints;
            while (n > 0)
            {
                double tempVolume;

                int noNondominatedPoints = FilterNondominatedSet(front, n, noObjectives - 1);
                //noNondominatedPoints = front.length;
                if (noObjectives < 3)
                {
                    if (noNondominatedPoints < 1)
                    {
                        throw new Exception("The number of dominate points is lesser than 1");
                    }

                    tempVolume = front[0][0];
                }
                else
                {
                    tempVolume = CalculateHypervolume(front,
                                                      noNondominatedPoints,
                                                      noObjectives - 1);
                }

                double tempDistance = SurfaceUnchangedTo(front, n, noObjectives - 1);
                volume += tempVolume*(tempDistance - distance);
                distance = tempDistance;
                n = ReduceNondominatedSet(front, n, noObjectives - 1, distance);
            }
            return volume;
        } // CalculateHypervolume
        /* merge two fronts */

        private double[][] MergeFronts(double[][] front1, int sizeFront1,
                                       double[][] front2, int sizeFront2, int noObjectives)
        {
            int i, j;

            /* allocate memory */
            int noPoints = sizeFront1 + sizeFront2;
            var frontPtr = new double[noPoints][];
            for (int index = 0; index < noPoints; index++)
            {
                frontPtr[index] = new double[noObjectives];
            }
            /* copy points */
            noPoints = 0;
            for (i = 0; i < sizeFront1; i++)
            {
                for (j = 0; j < noObjectives; j++)
                {
                    frontPtr[noPoints][j] = front1[i][j];
                }
                noPoints++;
            }
            for (i = 0; i < sizeFront2; i++)
            {
                for (j = 0; j < noObjectives; j++)
                {
                    frontPtr[noPoints][j] = front2[i][j];
                }
                noPoints++;
            }

            return frontPtr;
        } // MergeFronts
        /** 
   * Returns the hypevolume value of the paretoFront. This method call to the
   * calculate hipervolume one
   * @param paretoFront The pareto front
   * @param paretoTrueFront The true pareto front
   * @param numberOfObjectives Number of objectives of the pareto front
   */

        public double HypervolumeValue(double[][] paretoFront,
                                       double[][] paretoTrueFront,
                                       int numberOfObjectives)
        {
            /**
     * Stores the maximum values of true pareto front.
     */
            double[] maximumValues;

            /**
     * Stores the minimum values of the true pareto front.
     */
            double[] minimumValues;

            /**
     * Stores the normalized front.
     */
            double[][] normalizedFront;

            /**
     * Stores the inverted front. Needed for minimization problems
     */
            double[][] invertedFront;

            // STEP 1. Obtain the maximum and minimum values of the Pareto front
            maximumValues = MetricsUtil.GetMaximumValues(paretoTrueFront, numberOfObjectives);
            minimumValues = MetricsUtil.GetMinimumValues(paretoTrueFront, numberOfObjectives);

            // STEP 2. Get the normalized front
            normalizedFront = MetricsUtil.GetNormalizedFront(paretoFront,
                                                             maximumValues,
                                                             minimumValues);

            // STEP 3. Inverse the pareto front. This is needed because of the original
            //metric by Zitzler is for maximization problems
            invertedFront = MetricsUtil.InvertedFront(normalizedFront);

            // STEP4. The hypervolumen (control is passed to java version of Zitzler code)
            return CalculateHypervolume(invertedFront, invertedFront.Length, numberOfObjectives);
        }
    }
}