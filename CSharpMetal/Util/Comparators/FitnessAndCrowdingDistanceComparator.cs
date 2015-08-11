// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;

namespace CSharpMetal.Util.Comparators
{
    internal class FitnessAndCrowdingDistanceComparator : IComparer
    {
        /** 
 * stores a comparator for check the fitness value of the solutions
 */

        private static readonly IComparer FitnessComparator =
            new FitnessComparator();

        /** 
   * stores a comparator for check the crowding distance
   */

        private static readonly IComparer CrowdingDistanceComparator =
            new CrowdingDistanceComparator();

        public int Compare(object solution1, object solution2)
        {
            int flag = FitnessComparator.Compare(solution1, solution2);
            return flag != 0 ? flag : CrowdingDistanceComparator.Compare(solution1, solution2);
        }
    }
}