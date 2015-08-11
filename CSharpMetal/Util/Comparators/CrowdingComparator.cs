// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class CrowdingComparator : IComparer
    {
        private static readonly IComparer comparator = new RankComparator();

        public int Compare(object o1, object o2)
        {
            if (o1 == null)
            {
                return 1;
            }
            if (o2 == null)
            {
                return -1;
            }

            int flagComparatorRank = comparator.Compare(o1, o2);
            if (flagComparatorRank != 0)
            {
                return flagComparatorRank;
            }

            /* His rank is equal, then distance crowding comparator */
            double distance1 = ((Solution) o1).CrowdingDistance;
            double distance2 = ((Solution) o2).CrowdingDistance;
            if (distance1 > distance2)
            {
                return -1;
            }

            if (distance1 < distance2)
            {
                return 1;
            }

            return 0;
        }
    }
}