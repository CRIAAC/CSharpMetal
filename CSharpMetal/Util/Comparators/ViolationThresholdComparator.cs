// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class ViolationThresholdComparator : ConstraintViolationComparator
    {
        private double _threshold;

        public override int Compare(object o1, object o2)
        {
            double overall1 = ((Solution) o1).NumberOfViolatedConstraints*
                              ((Solution) o1).OverallConstraintViolation;
            double overall2 = ((Solution) o2).NumberOfViolatedConstraints*
                              ((Solution) o2).OverallConstraintViolation;

            if ((overall1 < 0) && (overall2 < 0))
            {
                if (overall1 > overall2)
                {
                    return -1;
                }
                if (overall2 > overall1)
                {
                    return 1;
                }
                return 0;
            }
            if ((Math.Abs(overall1) < double.Epsilon) && (overall2 < 0))
            {
                return -1;
            }
            if ((overall1 < 0) && (Math.Abs(overall2) < double.Epsilon))
            {
                return 1;
            }
            return 0;
        }

        public override bool NeedToCompare(Solution solution1, Solution solution2)
        {
            double overall1 = Math.Abs(solution1.NumberOfViolatedConstraints*
                                       solution1.OverallConstraintViolation);
            double overall2 = Math.Abs(solution2.NumberOfViolatedConstraints*
                                       solution2.OverallConstraintViolation);

            bool needToCompare = (overall1 > _threshold) || (overall2 > _threshold);

            return needToCompare;
        }

        /**
   * Computes the feasibility ratio
   * Return the ratio of feasible solutions
   */

        public double FeasibilityRatio(SolutionSet solutionSet)
        {
            double aux = 0.0;
            for (int i = 0; i < solutionSet.Size(); i++)
            {
                if (solutionSet[i].OverallConstraintViolation < 0)
                {
                    aux = aux + 1.0;
                }
            }
            return aux/solutionSet.Size();
        } // feasibilityRatio
        /**
         * Computes the feasibility ratio
         * Return the ratio of feasible solutions
         */

        public double MeanOveralViolation(SolutionSet solutionSet)
        {
            double aux = 0.0;
            for (int i = 0; i < solutionSet.Size(); i++)
            {
                aux += Math.Abs(solutionSet[i].NumberOfViolatedConstraints*
                                solutionSet[i].OverallConstraintViolation);
            }
            return aux/solutionSet.Size();
        } // meanOveralViolation
        /**
         * Updates the threshold value using the population
         */

        public void UpdateThreshold(SolutionSet set)
        {
            _threshold = FeasibilityRatio(set)*MeanOveralViolation(set);
        }
    }
}