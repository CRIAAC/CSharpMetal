﻿// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using CSharpMetal.Core;
using CSharpMetal.Encodings.Variables;

namespace CSharpMetal.Encodings.SolutionsType
{
    internal class PermutationSolutionType : BaseSolutionType
    {
        public PermutationSolutionType(Problem problem) : base(problem)
        {
        }

        public override BaseVariable[] CreateVariables()
        {
            BaseVariable[] variables = new BaseVariable[Problema.NumberOfVariables];

            for (int var = 0; var < Problema.NumberOfVariables; var++)
            {
                variables[var] = new Permutation(Problema.GetLength(var));
            }

            return variables;
        }
    }
}