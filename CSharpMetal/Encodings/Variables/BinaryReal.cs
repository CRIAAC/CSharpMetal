// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Util.CustomType;

namespace CSharpMetal.Encodings.Variables
{
    public class BinaryReal : Binary
    {
        public const int DefaultPrecision = 30;

        public override sealed double Value
        {
            get { return base.Value; }
            set
            {
                if (NumberOfBits <= 24 && LowerBound >= 0)
                {
                    BitSet bitSet;
                    if (value <= LowerBound)
                    {
                        bitSet = new BitSet(NumberOfBits);
                        bitSet.Clear();
                    }
                    else if (value >= UpperBound)
                    {
                        bitSet = new BitSet(NumberOfBits);
                        bitSet.Set(0, NumberOfBits);
                    }
                    else
                    {
                        bitSet = new BitSet(NumberOfBits);
                        bitSet.Clear();
                        // value is the integerToCode-th possible value, what is integerToCode?
                        int integerToCode = 0;
                        double tmp = LowerBound;
                        double path = (UpperBound - LowerBound)/(Math.Pow(2.0, NumberOfBits) - 1);
                        while (tmp < value)
                        {
                            tmp += path;
                            integerToCode++;
                        }
                        int remain = integerToCode;
                        for (int i = NumberOfBits - 1; i >= 0; i--)
                        {
                            int ithPowerOf2 = (int) Math.Pow(2, i);

                            if (ithPowerOf2 <= remain)
                            {
                                bitSet.Set(i);
                                remain -= ithPowerOf2;
                            }
                            else
                            {
                                bitSet.Clear(i);
                            }
                        }
                    }
                    Bits = bitSet;
                    Decode();
                }
                else
                {
                    if (LowerBound < 0)
                    {
                        throw new Exception("Unsupported lowerbound: " + LowerBound
                                            + " > 0");
                    }
                    if (NumberOfBits >= 24)
                    {
                        throw new Exception("Unsupported bit string length"
                                            + NumberOfBits + " is > 24 bits");
                    }
                }
            }
        }

        public override sealed double LowerBound
        {
            get { return base.LowerBound; }
            set { base.LowerBound = value; }
        }

        public override sealed double UpperBound
        {
            get { return base.UpperBound; }
            set { base.UpperBound = value; }
        }

        public BinaryReal(int numberOfBits, double lowerBound, double upperBound) : base(numberOfBits)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;

            Decode();
        }

        public BinaryReal(BitSet bits, int numberOfBits, double lowerBound, double upperBound)
            : base(numberOfBits)
        {
            Bits = bits.Clone() as BitSet;
            LowerBound = lowerBound;
            UpperBound = upperBound;

            Decode();
        }

        public BinaryReal(BinaryReal binaryReal)
            : this(binaryReal.Bits, binaryReal.NumberOfBits, binaryReal.LowerBound, binaryReal.UpperBound)
        {
            Value = binaryReal.Value;
        }

        public override sealed void Decode()
        {
            double value = 0.0;
            for (int i = 0; i < NumberOfBits; i++)
            {
                if (Bits.Get(i))
                {
                    value += Math.Pow(2.0, i);
                }
            }

            Value = value*(UpperBound - LowerBound)/
                    (Math.Pow(2.0, NumberOfBits) - 1.0);
            Value += LowerBound;
        }

        public override BaseVariable Clone()
        {
            return new BinaryReal(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}