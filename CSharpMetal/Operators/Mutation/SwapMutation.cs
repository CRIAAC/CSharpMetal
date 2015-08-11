// Author : Vandewynckel Julien
// Creation date : 09/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Encodings.Variables;
using CSharpMetal.Util;

namespace CSharpMetal.Operators.Mutation
{
    internal class SwapMutation : BaseMutation
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (PermutationSolutionType)
        };

        private readonly double _mutationProbability;

        public SwapMutation(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object parameter;
            if (parameters.TryGetValue("probability", out parameter))
            {
                _mutationProbability = (double) parameter;
            }
            else
            {
                throw new Exception("mutationProbability_ is a NaN");
            }
        }

        public void DoMutation(double probability, Solution solution)
        {
            if (solution.SolutionType.GetType() == typeof (PermutationSolutionType))
            {
                int permutationLength = ((Permutation) solution.DecisionVariables[0]).Size;
                int[] permutation = ((Permutation) solution.DecisionVariables[0]).Vector;

                if (PseudoRandom.Instance().NextDouble() < probability)
                {
                    int pos1 = PseudoRandom.Instance().Next(0, permutationLength - 1);
                    int pos2 = PseudoRandom.Instance().Next(0, permutationLength - 1);

                    while (pos1 == pos2)
                    {
                        pos2 = pos1 == (permutationLength - 1)
                                   ? PseudoRandom.Instance().Next(0, permutationLength - 2)
                                   : PseudoRandom.Instance().Next(pos1, permutationLength - 1);
                    }
                    int temp = permutation[pos1];
                    permutation[pos1] = permutation[pos2];
                    permutation[pos2] = temp;
                }
            }
            else
            {
                throw new Exception("Exception in " + GetType().Name + ".doMutation()");
            }
        }

        public override object Execute(object obj)
        {
            Solution solution = (Solution) obj;

            if (!ValidTypes.Contains(solution.SolutionType.GetType()))
            {
                throw new Exception("the solution type " + solution.SolutionType.GetType() +
                                    " is not of the right type. The type should be 'Binary'," +
                                    "'BinaryReal' or 'Int', but " + solution.SolutionType.GetType() + " is obtained");
            }

            DoMutation(_mutationProbability, solution);

            return solution;
        }
    }
}