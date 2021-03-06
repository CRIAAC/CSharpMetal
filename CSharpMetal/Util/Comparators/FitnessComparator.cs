﻿// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;

namespace CSharpMetal.Util.Comparators
{
    internal class FitnessComparator : IComparer
    {
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

            double fitness1 = ((Solution) o1).Fitness;
            double fitness2 = ((Solution) o2).Fitness;
            if (fitness1 < fitness2)
            {
                return -1;
            }

            if (fitness1 > fitness2)
            {
                return 1;
            }

            return 0;
        }
    }
}