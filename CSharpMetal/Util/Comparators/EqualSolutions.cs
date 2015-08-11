// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    public class EqualSolutions : IComparer
    {
        public int Compare(object object1, object object2)
        {
            if (object1 == null)
            {
                return 1;
            }
            if (object2 == null)
            {
                return -1;
            }

            int dominate1; // dominate1 indicates if some objective of solution1 
            // dominates the same objective in solution2. dominate2
            int dominate2; // is the complementary of dominate1.

            dominate1 = 0;
            dominate2 = 0;

            Solution solution1 = (Solution) object1;
            Solution solution2 = (Solution) object2;

            int flag;
            double value1, value2;
            for (int i = 0; i < solution1.NumberOfObjectives; i++)
            {
                flag = (new ObjectiveComparator(i)).Compare(solution1, solution2);
                value1 = solution1.Objective[i];
                value2 = solution2.Objective[i];

                if (value1 < value2)
                {
                    flag = -1;
                }
                else if (value1 > value2)
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

            if (dominate1 == 0 && dominate2 == 0)
            {
                return 0; //No one dominate the other
            }

            if (dominate1 == 1)
            {
                return -1; // solution1 dominate
            }
            if (dominate2 == 1)
            {
                return 1; // solution2 dominate
            }
            return 2;
        }
    }
}