// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Linq;

namespace CSharpMetal.Core
{
    public abstract class BaseSolutionType
    {
        public Problem Problema { get; set; }

        public BaseSolutionType(Problem problem)
        {
            Problema = problem;
        }

        /*
        public BaseSolutionType Clone()
        {
            var copy = (BaseSolutionType) MemberwiseClone();
            copy.Problema = Problema.Clone();
            return copy;
        }*/

        public abstract BaseVariable[] CreateVariables();

        public BaseVariable[] CopyVariables(BaseVariable[] vars)
        {
            if (vars == null)
            {
                throw new ArgumentNullException("vars");
            }

            return vars.Select(a => a.Clone()).ToArray();
        }
    }
}