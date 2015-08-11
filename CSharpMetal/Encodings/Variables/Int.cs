// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Util;

namespace CSharpMetal.Encodings.Variables
{
    internal class Int : BaseVariable
    {
        public int IntValue { get; set; }
        public int IntUpperBound { get; set; }
        public int IntLowerBound { get; set; }

        public override double Value
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override double LowerBound
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override double UpperBound
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Int()
        {
            IntLowerBound = Int32.MinValue;
            IntUpperBound = Int32.MaxValue;
            IntValue = 0;
        }

        public Int(int lowerBound, int upperBound)
        {
            IntLowerBound = lowerBound;
            IntUpperBound = upperBound;
            IntValue = PseudoRandom.Instance().Next(lowerBound, upperBound);
        }

        public Int(int value, int lowerBound, int upperBound)
        {
            IntValue = value;
            IntLowerBound = lowerBound;
            IntUpperBound = upperBound;
        }

        public Int(Int variable)
        {
            IntValue = variable.IntValue;
            IntLowerBound = variable.IntLowerBound;
            IntUpperBound = variable.IntUpperBound;
        }

        public override BaseVariable Clone()
        {
            return new Int(this);
        }

        public override string ToString()
        {
            return IntValue + "";
        }
    }
}