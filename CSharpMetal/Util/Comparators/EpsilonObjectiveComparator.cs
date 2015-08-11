// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class EpsilonObjectiveComparator : IComparer
    {
        /**
         * Stores the eta value for epsilon-dominance
         */
        private readonly double _eta;
        /**
   * Stores the objective index to compare
   */
        private readonly int _objective;

        public EpsilonObjectiveComparator(int nObj, double eta)
        {
            _objective = nObj;
            _eta = eta;
        }

        public int Compare(Object o1, Object o2)
        {
            if (o1 == null)
            {
                return 1;
            }
            if (o2 == null)
            {
                return -1;
            }

            double objetive1 = ((Solution) o1).Objective[_objective];
            double objetive2 = ((Solution) o2).Objective[_objective];

            //Objetive implements comparable!!! 
            if (objetive1/(1 + _eta) < objetive2)
            {
                return -1;
            }
            if (objetive1/(1 + _eta) > objetive2)
            {
                return 1;
            }
            return 0;
        }
    }
}