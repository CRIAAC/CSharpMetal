// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Clonable;

namespace CSharpMetal.Encodings.Variables
{
    public class ArrayReal : BaseVariable, ITCloneable<ArrayReal>
    {
        public Problem Problema { get; set; }
        public int Size { get; set; }
        public double[] DoubleValues { get; set; }

        public override double Value
        {
            get { throw new Exception("This property does not exist"); }
            set { throw new Exception("This property does not exist"); }
        }

        public override double LowerBound
        {
            get { throw new Exception("This property does not exist"); }
            set { throw new Exception("This property does not exist"); }
        }

        public override double UpperBound
        {
            get { throw new Exception("This property does not exist"); }
            set { throw new Exception("This property does not exist"); }
        }

        public ArrayReal()
        {
            Problema = null;
            Size = 0;
            DoubleValues = null;
        }

        public ArrayReal(int size, Problem problem)
        {
            Problema = problem;
            Size = size;
            DoubleValues = new double[Size];

            for (var i = 0; i < Size; i++)
            {
                DoubleValues[i] = PseudoRandom.Instance().NextDouble()*(Problema.UpperLimit[i] -
                                                                        Problema.LowerLimit[i]) + problem.LowerLimit[i];
            }
        }

        public ArrayReal(ArrayReal arrayReal)
        {
            Problema = arrayReal.Problema;
            Size = arrayReal.Size;
            DoubleValues = new double[arrayReal.Size];
            Array.Copy(arrayReal.DoubleValues, DoubleValues, arrayReal.Size);
        }

        ArrayReal ITCloneable<ArrayReal>.Clone()
        {
            return new ArrayReal(this);
        }

        public override BaseVariable Clone()
        {
            return new ArrayReal(this);
        }

        public override string ToString()
        {
            return string.Join(",", DoubleValues);
        }

        public double GetLowerBound(int index)
        {
            return Problema.LowerLimit[index];
        }

        public double GetUpperBound(int index)
        {
            return Problema.UpperLimit[index];
        }
    }
}