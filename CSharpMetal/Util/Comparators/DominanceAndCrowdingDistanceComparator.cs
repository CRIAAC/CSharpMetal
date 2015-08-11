// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;

namespace CSharpMetal.Util.Comparators
{
    public class DominanceAndCrowdingDistanceComparator : IComparer
    {
        private static readonly IComparer dominance = new DominanceComparator();
        private static readonly IComparer crowding = new CrowdingDistanceComparator();

        int IComparer.Compare(object x, object y)
        {
            int result;

            result = dominance.Compare(x, y);
            if (result == 0)
            {
                result = crowding.Compare(x, y);
            }

            return result;
        }
    }
}