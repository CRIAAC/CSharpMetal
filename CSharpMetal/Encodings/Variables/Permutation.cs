// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;

namespace CSharpMetal.Encodings.Variables
{
    internal class Permutation : BaseVariable
    {
        public int[] Vector { get; set; }
        public int Size { get; set; }

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

        public Permutation()
        {
            Size = 0;
            Vector = null;
        }

        public Permutation(int size)
        {
            Size = size;
            Vector = new int[Size];

            List<int> randomSequence = new List<int>(Size);

            for (int i = 0; i < Size; i++)
            {
                randomSequence.Add(i);
            }

            Shuffle(randomSequence);

            for (int j = 0; j < randomSequence.Count; j++)
            {
                Vector[j] = randomSequence[j];
            }
        }

        public Permutation(Permutation permutation)
        {
            Size = permutation.Size;
            Vector = new int[Size];

            Array.Copy(permutation.Vector, 0, Vector, 0, Size);
        }

        private static void Shuffle<T>(IList<T> array)
        {
            int n = array.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int) (PseudoRandom.Instance().NextDouble()*(n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public override BaseVariable Clone()
        {
            return new Permutation(this);
        }

        public override string ToString()
        {
            return string.Join(",", Vector);
        }
    }
}