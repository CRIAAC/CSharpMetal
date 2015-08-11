// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    public class ObjectiveComparator : IComparer
    {
        private readonly bool _ascendingOrder;
        private readonly int _nObj;

        public ObjectiveComparator(int nObj)
        {
            _nObj = nObj;
            _ascendingOrder = true;
        } // ObjectiveComparator

        public ObjectiveComparator(int nObj, bool descendingOrder)
        {
            _nObj = nObj;
            _ascendingOrder = !descendingOrder;
        } // ObjectiveComparator
        /**
         * Compares two solutions.
         *
         * @param o1 Object representing the first <code>Solution</code>.
         * @param o2 Object representing the second <code>Solution</code>.
         * @return -1, or 0, or 1 if o1 is less than, equal, or greater than o2,
         *         respectively.
         */

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

            double objetive1 = ((Solution) o1).Objective[_nObj];
            double objetive2 = ((Solution) o2).Objective[_nObj];
            if (_ascendingOrder)
            {
                if (objetive1 < objetive2)
                {
                    return -1;
                }
                if (objetive1 > objetive2)
                {
                    return 1;
                }
                return 0;
            }
            if (objetive1 < objetive2)
            {
                return 1;
            }
            if (objetive1 > objetive2)
            {
                return -1;
            }
            return 0;
        }
    }
}