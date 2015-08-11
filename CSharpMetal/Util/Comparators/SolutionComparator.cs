// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class SolutionComparator : IComparer
    {
        private const double Epsilon = 1e-10;

        public int Compare(object o1, object o2)
        {
            var solution1 = (Solution) o1;
            var solution2 = (Solution) o2;

            if ((solution1.DecisionVariables != null) && (solution2.DecisionVariables != null))
            {
                if (solution1.NumberOfVariables != solution2.NumberOfVariables)
                {
                    return -1;
                }
            }

            if (Distance.DistanceBetweenSolutions(solution1, solution2) < Epsilon)
            {
                return 0;
            }
            return -1;
        }
    }
}