// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using System.Linq;
using CSharpMetal.QualityIndicators.Util;

namespace CSharpMetal.QualityIndicators
{
    internal class InvertedGenerationalDistance
    {
        private const double Exponent = 2.0; //pow. This is the pow used for the
        //distances


        /**
   * Returns the inverted generational distance value for a given front
   * @param front The front 
   * @param trueParetoFront The true pareto front
   */

        public double Compute(double[][] front,
                              double[][] trueParetoFront,
                              int numberOfObjectives)
        {
            // STEP 1. Obtain the maximum and minimum values of the Pareto front
            double[] maximumValue = MetricsUtil.GetMaximumValues(trueParetoFront, numberOfObjectives);
            double[] minimumValue = MetricsUtil.GetMinimumValues(trueParetoFront, numberOfObjectives);

            // STEP 2. Get the normalized front and true Pareto fronts
            double[][] normalizedFront = MetricsUtil.GetNormalizedFront(front,
                                                                        maximumValue,
                                                                        minimumValue);
            double[][] normalizedParetoFront = MetricsUtil.GetNormalizedFront(trueParetoFront,
                                                                              maximumValue,
                                                                              minimumValue);

            // STEP 3. Sum the distances between each point of the true Pareto front and
            // the nearest point in the true Pareto front
            double sum =
                normalizedParetoFront.Sum(
                    aNormalizedParetoFront =>
                    Math.Pow(MetricsUtil.DistanceToClosedPoint(aNormalizedParetoFront, normalizedFront), Exponent));


            // STEP 4. Obtain the sqrt of the sum
            sum = Math.Pow(sum, 1.0/Exponent);

            // STEP 5. Divide the sum by the maximum number of points of the front
            double generationalDistance = sum/normalizedParetoFront.Length;

            return generationalDistance;
        }
    }
}