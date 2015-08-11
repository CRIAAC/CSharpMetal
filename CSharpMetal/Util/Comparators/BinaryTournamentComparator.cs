// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class BinaryTournamentComparator : IComparer
    {
        private static readonly IComparer Dominance = new DominanceComparator();

        /// <summary>
        ///     Compares two solutions.
        ///     A <code>Solution</code> a is less than b for this <code>Comparator</code>.
        ///     if the crowding distance of a if greater than the crowding distance of b.
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public int Compare(object o1, object o2)
        {
            int flag = Dominance.Compare(o1, o2);
            if (flag != 0)
            {
                return flag;
            }

            double crowding1 = ((Solution) o1).CrowdingDistance;
            double crowding2 = ((Solution) o2).CrowdingDistance;

            if (crowding1 > crowding2)
            {
                return -1;
            }
            if (crowding2 > crowding1)
            {
                return 1;
            }
            return 0;
        }
    }
}