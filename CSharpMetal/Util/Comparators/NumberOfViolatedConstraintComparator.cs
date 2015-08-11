// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class NumberOfViolatedConstraintComparator : ConstraintViolationComparator
    {
        public override int Compare(object o1, object o2)
        {
            Solution solution1 = (Solution) o1;
            Solution solution2 = (Solution) o2;

            if (solution1.NumberOfViolatedConstraints <
                solution2.NumberOfViolatedConstraints)
            {
                return -1;
            }
            if (solution2.NumberOfViolatedConstraints <
                solution1.NumberOfViolatedConstraints)
            {
                return 1;
            }

            return 0;
        }

        public override bool NeedToCompare(Solution solution1, Solution solution2)
        {
            return (solution1.NumberOfViolatedConstraints > 0) ||
                   (solution2.NumberOfViolatedConstraints > 0);
        }
    }
}