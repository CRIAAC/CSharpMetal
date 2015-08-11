// Author : Vandewynckel Julien
// Creation date : 13/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;

namespace CSharpMetal.Util.Comparators
{
    internal class ThetaComparator : IComparer
    {
        public double THETA { get; set; }

        public ThetaComparator(double theta)
        {
            THETA = theta;
        }

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


            if (o1 is Tuple<double, double> && o2 is Tuple<double, double>)
            {
                Tuple<double, double> tuple1 = o1 as Tuple<double, double>;
                Tuple<double, double> tuple2 = o2 as Tuple<double, double>;

                var f1 = tuple1.Item1 + THETA*tuple1.Item2;
                var f2 = tuple2.Item1 + THETA*tuple2.Item2;
                var returnCompare = f1.CompareTo(f2);

                if (returnCompare != 0)
                {
                    return returnCompare;
                }
                if (PseudoRandom.Instance().NextDouble() > 0.5)
                {
                    return 1;
                }
                return -1;
            }
            throw new Exception("Theta Comparator take only two tuples");
        }
    }
}