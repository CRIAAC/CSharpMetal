// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    public class CrowdingDistanceComparator : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            double distance1 = ((Solution) x).CrowdingDistance;
            double distance2 = ((Solution) y).CrowdingDistance;
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