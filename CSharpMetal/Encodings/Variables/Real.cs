// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Clonable;

namespace CSharpMetal.Encodings.Variables
{
    public class Real : BaseVariable, ITCloneable<Real>
    {
        public override double Value { get; set; }
        public override double LowerBound { get; set; }
        public override double UpperBound { get; set; }

        public Real(double value, double lowerBound, double upperBound)
        {
            Value = value;
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public Real(double lowerBound, double upperBound) :
            this(PseudoRandom.Instance().NextDouble()*(upperBound - lowerBound) + lowerBound, lowerBound, upperBound)
        {
        }

        public Real(BaseVariable variable)
            : this(variable.Value, variable.LowerBound, variable.UpperBound)
        {
        }

        public Real(Real real)
            : this(real.Value, real.LowerBound, real.UpperBound)
        {
        }

        Real ITCloneable<Real>.Clone()
        {
            return new Real(Value, LowerBound, UpperBound);
        }

        public override BaseVariable Clone()
        {
            return new Real(Value, LowerBound, UpperBound);
        }

        public override string ToString()
        {
            return Value + ";" + LowerBound + ";" + UpperBound;
        }
    }
}