// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    public abstract class ConstraintViolationComparator : IComparer
    {
        public abstract int Compare(object o1, object o2);
        public abstract bool NeedToCompare(Solution solution1, Solution solution2);
    }
}