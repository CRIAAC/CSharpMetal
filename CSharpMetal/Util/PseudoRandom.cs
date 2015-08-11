// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;

namespace CSharpMetal.Util
{
    public class PseudoRandom : Random
    {
        private static readonly object mutex_ = new object();
        private static PseudoRandom PseudoRandom_;

        private PseudoRandom()
        {
            // normal initialization, do not call Instance()
        }

        public static PseudoRandom Instance()
        {
            if (PseudoRandom_ == null)
            {
                lock (mutex_)
                {
                    if (PseudoRandom_ == null)
                    {
                        PseudoRandom_ = new PseudoRandom();
                    }
                }
            }
            return PseudoRandom_;
        }
    }
}