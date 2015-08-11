// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Encodings.Variables;

namespace CSharpMetal.Encodings.SolutionsType
{
    internal class RealArraySolutionType : BaseSolutionType
    {
        public RealArraySolutionType(Problem problem)
            : base(problem)
        {
            if (problem == null)
            {
                throw new ArgumentNullException("problem");
            }

            problem.VariableType = new Type[problem.NumberOfVariables];
            problem.TypeOfSolution = this;

            // Initializing the types of the variables
            for (var i = 0; i < problem.NumberOfVariables; i++)
            {
                problem.VariableType[i] = typeof (ArrayReal);
            }
        }

        public override BaseVariable[] CreateVariables()
        {
            var variables = new BaseVariable[Problema.NumberOfVariables];

            for (var var = 0; var < Problema.NumberOfVariables; var++)
            {
                variables[var] = new ArrayReal(Problema.NumberOfVariables, Problema);
            }

            return variables;
        }
    }
}