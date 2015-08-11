// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.QualityIndicators.Util;

namespace CSharpMetal.QualityIndicators
{
    internal class Spread
    {
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

            // STEP 3. Sort normalizedFront and normalizedParetoFront;
            Array.Sort(normalizedFront, new
                                            LexicoGraphicalComparator());
            Array.Sort(normalizedParetoFront, new
                                                  LexicoGraphicalComparator());

            int numberOfPoints = normalizedFront.Length;
            //    int numberOfTruePoints = normalizedParetoFront.Length;

            // STEP 4. Compute df and dl (See specifications in Deb's description of 
            // the metric)
            double df = MetricsUtil.EuclideanDistance(normalizedFront[0], normalizedParetoFront[0]);
            double dl = MetricsUtil.EuclideanDistance(normalizedFront[normalizedFront.Length - 1],
                                                      normalizedParetoFront[normalizedParetoFront.Length - 1]);

            double mean = 0.0;
            double diversitySum = df + dl;

            // STEP 5. Calculate the mean of EuclideanDistances between points i and (i - 1). 
            // (the poins are in lexicografical order)
            for (int i = 0; i < (normalizedFront.Length - 1); i++)
            {
                mean += MetricsUtil.EuclideanDistance(normalizedFront[i], normalizedFront[i + 1]);
            } // for

            mean = mean/(numberOfPoints - 1);

            // STEP 6. If there are more than a single point, continue computing the 
            // metric. In other case, return the worse value (1.0, see metric's 
            // description).
            if (numberOfPoints > 1)
            {
                for (int i = 0; i < (numberOfPoints - 1); i++)
                {
                    diversitySum += Math.Abs(MetricsUtil.EuclideanDistance(normalizedFront[i],
                                                                           normalizedFront[i + 1]) - mean);
                } // for
                return diversitySum/(df + dl + (numberOfPoints - 1)*mean);
            }
            return 1.0;
        }
    }
}