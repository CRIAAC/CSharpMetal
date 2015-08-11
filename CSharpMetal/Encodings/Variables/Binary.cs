// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Clonable;
using CSharpMetal.Util.CustomType;

namespace CSharpMetal.Encodings.Variables
{
    public class Binary : BaseVariable, ITCloneable<Binary>
    {
        public int NumberOfBits { get; set; }
        public BitSet Bits { get; set; }

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

        public Binary()
        {
        }

        public Binary(int numberOfBits)
        {
            NumberOfBits = numberOfBits;

            Bits = new BitSet(numberOfBits);

            for (var i = 0; i < numberOfBits; i++)
            {
                if (PseudoRandom.Instance().NextDouble() < 0.5)
                {
                    Bits.Set(i, true);
                }
                else
                {
                    Bits.Set(i, false);
                }
            }
        } //Binary

        public Binary(Binary variable)
        {
            NumberOfBits = variable.NumberOfBits;

            Bits = new BitSet(NumberOfBits);
            for (var i = 0; i < NumberOfBits; i++)
            {
                Bits.Set(i, variable.Bits.Get(i));
            }
        }

        Binary ITCloneable<Binary>.Clone()
        {
            return new Binary(this);
        }

        public int HammingDistance(Binary other)
        {
            int distance = 0;
            int i = 0;
            while (i < Bits.Length)
            {
                if (Bits.Get(i) != other.Bits.Get(i))
                {
                    distance++;
                }
                i++;
            }
            return distance;
        }

        public override BaseVariable Clone()
        {
            return new Binary(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public virtual void Decode()
        {
        }
    }
}