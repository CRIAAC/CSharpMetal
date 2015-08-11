// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class RankComparator : IComparer
    {
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

            Solution solution1 = (Solution) o1;
            Solution solution2 = (Solution) o2;
            if (solution1.Rank < solution2.Rank)
            {
                return -1;
            }

            if (solution1.Rank > solution2.Rank)
            {
                return 1;
            }

            return 0;
        }
    }
}