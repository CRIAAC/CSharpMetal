// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.QualityIndicators.Util;

namespace CSharpMetal.QualityIndicators
{
    internal class GenerationalDistance
    {
        private const double Exponent = 2.0; //pow. This is the pow used for the
        //distances

        /**
   * Returns the generational distance value for a given front
   * @param front The front 
   * @param trueParetoFront The true pareto front
   */

        public double Compute(double[][] front,
                              double[][] trueParetoFront,
                              int numberOfObjectives)
        {
            /**
     * Stores the maximum values of true pareto front.
     */
            double[] maximumValue;

            /**
     * Stores the minimum values of the true pareto front.
     */
            double[] minimumValue;

            /**
     * Stores the normalized front.
     */
            double[][] normalizedFront;

            /**
     * Stores the normalized true Pareto front.
     */
            double[][] normalizedParetoFront;

            // STEP 1. Obtain the maximum and minimum values of the Pareto front
            maximumValue = MetricsUtil.GetMaximumValues(trueParetoFront, numberOfObjectives);
            minimumValue = MetricsUtil.GetMinimumValues(trueParetoFront, numberOfObjectives);

            // STEP 2. Get the normalized front and true Pareto fronts
            normalizedFront = MetricsUtil.GetNormalizedFront(front,
                                                             maximumValue,
                                                             minimumValue);
            normalizedParetoFront = MetricsUtil.GetNormalizedFront(trueParetoFront,
                                                                   maximumValue,
                                                                   minimumValue);

            // STEP 3. Sum the distances between each point of the front and the 
            // nearest point in the true Pareto front
            double sum = 0.0;
            for (int i = 0; i < front.Length; i++)
            {
                sum += Math.Pow(MetricsUtil.DistanceToClosedPoint(normalizedFront[i],
                                                                  normalizedParetoFront),
                                Exponent);
            }


            // STEP 4. Obtain the sqrt of the sum
            sum = Math.Pow(sum, 1.0/Exponent);

            // STEP 5. Divide the sum by the maximum number of points of the front
            double generationalDistance = sum/normalizedFront.Length;

            return generationalDistance;
        }
    }
}