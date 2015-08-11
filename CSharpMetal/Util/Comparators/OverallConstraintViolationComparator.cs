// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    public class OverallConstraintViolationComparator : ConstraintViolationComparator
    {
        public override int Compare(object o1, object o2)
        {
            double overall1, overall2;
            overall1 = ((Solution) o1).OverallConstraintViolation;
            overall2 = ((Solution) o2).OverallConstraintViolation;

            if ((overall1 < 0) && (overall2 < 0))
            {
                if (overall1 > overall2)
                {
                    return -1;
                }
                if (overall2 > overall1)
                {
                    return 1;
                }
                return 0;
            }
            if ((Math.Abs(overall1) < Double.Epsilon) && (overall2 < 0))
            {
                return -1;
            }
            if ((overall1 < 0) && (Math.Abs(overall2) < Double.Epsilon))
            {
                return 1;
            }
            return 0;
        }

        public override bool NeedToCompare(Solution solution1, Solution solution2)
        {
            return (solution1.OverallConstraintViolation < 0) ||
                   (solution2.OverallConstraintViolation < 0);
        }
    }
}