// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Encodings.Variables;

namespace CSharpMetal.Encodings.SolutionsType
{
    public class BinaryRealSolutionType : BaseSolutionType
    {
        public BinaryRealSolutionType(Problem problem) : base(problem)
        {
        }

        public override BaseVariable[] CreateVariables()
        {
            BaseVariable[] variables = new BaseVariable[Problema.NumberOfVariables];

            for (int localVariable = 0; localVariable < Problema.NumberOfVariables; localVariable++)
            {
                if (Problema.Precision == null)
                {
                    int[] precision = new int[Problema.NumberOfVariables];
                    for (int i = 0; i < Problema.NumberOfVariables; i++)
                    {
                        precision[i] = BinaryReal.DefaultPrecision;
                    }
                    Problema.Precision = new int[precision.Length];
                    Array.Copy(precision, Problema.Precision, precision.Length);
                } // if
                variables[localVariable] = new BinaryReal(Problema.Precision[localVariable],
                                                          Problema.LowerLimit[localVariable],
                                                          Problema.UpperLimit[localVariable]);
            } // for 
            return variables;
        }
    }
}