// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class EpsilonDominanceComparator : IComparer
    {
        private static IComparer _overallConstraintViolationComparator =
            new OverallConstraintViolationComparator();

        public double Eta { get; private set; }

        public static IComparer ViolationComparator
        {
            get { return _overallConstraintViolationComparator; }
            set { _overallConstraintViolationComparator = value; }
        }

        public EpsilonDominanceComparator(double eta)
        {
            Eta = eta;
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

            int dominate1; // dominate1 indicates if some objective of solution1 
            // dominates the same objective in solution2. dominate2
            int dominate2; // is the complementary of dominate1.

            dominate1 = 0;
            dominate2 = 0;

            Solution solution1 = (Solution) o1;
            Solution solution2 = (Solution) o2;

            int flag;
            IComparer constraint = new OverallConstraintViolationComparator();
            flag = constraint.Compare(solution1, solution2);

            if (flag != 0)
            {
                return flag;
            }

            double value1, value2;
            // Idem number of violated constraint. Apply a dominance Test

            double nrObjectives = solution1.NumberOfObjectives;
            for (int i = 0; i < nrObjectives; i++)
            {
                value1 = solution1.Objective[i];
                value2 = solution2.Objective[i];

                //Objetive implements comparable!!! 
                if (value1/(1 + Eta) < value2)
                {
                    flag = -1;
                }
                else if (value1/(1 + Eta) > value2)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }

                if (flag == -1)
                {
                    dominate1 = 1;
                }

                if (flag == 1)
                {
                    dominate2 = 1;
                }
            }

            if (dominate1 == dominate2)
            {
                return 0; // No one dominates the other
            }

            if (dominate1 == 1)
            {
                return -1; // solution1 dominates
            }

            return 1; // solution2 dominates
        }
    }
}