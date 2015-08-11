// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class DistanceToPopulationComparator : IComparer
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

            double distance1 = ((Solution) o1).DistanceToSolutionSet;
            double distance2 = ((Solution) o2).DistanceToSolutionSet;
            if (distance1 < distance2)
            {
                return -1;
            }
            if (distance1 > distance2)
            {
                return 1;
            }

            return 0;
        }
    }
}