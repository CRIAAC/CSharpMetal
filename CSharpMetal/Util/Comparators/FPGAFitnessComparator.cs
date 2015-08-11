// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class FPGAFitnessComparator : IComparer
    {
        public int Compare(object o1, object o2)
        {
            Solution solution1, solution2;
            solution1 = (Solution) o1;
            solution2 = (Solution) o2;

            if (solution1.Rank == 0 && solution2.Rank > 0)
            {
                return -1;
            }
            if (solution1.Rank > 0 && solution2.Rank == 0)
            {
                return 1;
            }
            if (solution1.Fitness > solution2.Fitness)
            {
                return -1;
            }
            if (solution1.Fitness < solution2.Fitness)
            {
                return 1;
            }
            return 0;
        }
    }
}