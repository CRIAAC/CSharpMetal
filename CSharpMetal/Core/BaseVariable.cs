// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using CSharpMetal.Util.Clonable;

namespace CSharpMetal.Core
{
    public abstract class BaseVariable : ITCloneable<BaseVariable>
    {
        public abstract double Value { get; set; }
        public abstract double LowerBound { get; set; }
        public abstract double UpperBound { get; set; }
        public abstract BaseVariable Clone();
        public abstract override string ToString();
    }
}