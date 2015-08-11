// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;

namespace CSharpMetal.QualityIndicators.Util
{
    internal class LexicoGraphicalComparator : IComparer
    {
        public int Compare(object o1, object o2)
        {
            double[] pointOne = (double[]) o1;
            double[] pointTwo = (double[]) o2;

            //To determine the first i, that pointOne[i] != pointTwo[i];
            int index = 0;
            while ((index < pointOne.Length) && (index < pointTwo.Length) &&
                   Math.Abs(pointOne[index] - pointTwo[index]) < double.Epsilon)
            {
                index++;
            }
            if ((index >= pointOne.Length) || (index >= pointTwo.Length))
            {
                return 0;
            }
            if (pointOne[index] < pointTwo[index])
            {
                return -1;
            }
            if (pointOne[index] > pointTwo[index])
            {
                return 1;
            }
            return 0;
        }
    }
}