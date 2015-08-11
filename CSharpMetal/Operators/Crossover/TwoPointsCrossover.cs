// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Encodings.Variables;
using CSharpMetal.Util;

namespace CSharpMetal.Operators.Crossover
{
    internal class TwoPointsCrossover : Crossover
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (PermutationSolutionType)
        };

        private readonly double _crossoverProbability;

        public TwoPointsCrossover(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object parameter;
            if (parameters.TryGetValue("probability", out parameter))
            {
                _crossoverProbability = (double) parameter;
            }
            else
            {
                throw new Exception("crossoverProbability_ is a NaN");
            }
        }

        public Solution[] DoCrossover(double probability,
                                      Solution parent1,
                                      Solution parent2)
        {
            Solution[] offspring = new Solution[2];

            offspring[0] = new Solution(parent1);
            offspring[1] = new Solution(parent2);

            if (parent1.SolutionType.GetType() == typeof (PermutationSolutionType))
            {
                if (PseudoRandom.Instance().NextDouble() < probability)
                {
                    var permutationLength = ((Permutation) parent1.DecisionVariables[0]).Size;
                    var parent1Vector = ((Permutation) parent1.DecisionVariables[0]).Vector;
                    var parent2Vector = ((Permutation) parent2.DecisionVariables[0]).Vector;
                    var offspring1Vector = ((Permutation) offspring[0].DecisionVariables[0]).Vector;
                    var offspring2Vector = ((Permutation) offspring[1].DecisionVariables[0]).Vector;

                    // STEP 1: Get two cutting points
                    int crosspoint1 = PseudoRandom.Instance().Next(0, permutationLength - 1);
                    int crosspoint2 = PseudoRandom.Instance().Next(0, permutationLength - 1);

                    while (crosspoint2 == crosspoint1)
                    {
                        crosspoint2 = PseudoRandom.Instance().Next(0, permutationLength - 1);
                    }

                    if (crosspoint1 > crosspoint2)
                    {
                        int swap = crosspoint1;
                        crosspoint1 = crosspoint2;
                        crosspoint2 = swap;
                    }

                    // STEP 2: Obtain the first child
                    int m = 0;
                    foreach (var j in parent2Vector)
                    {
                        bool exist = false;
                        int temp = j;
                        for (int k = crosspoint1; k <= crosspoint2; k++)
                        {
                            if (temp == offspring1Vector[k])
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            if (m == crosspoint1)
                            {
                                m = crosspoint2 + 1;
                            }
                            offspring1Vector[m++] = temp;
                        }
                    }

                    // STEP 3: Obtain the second child
                    m = 0;
                    foreach (var j in parent1Vector)
                    {
                        bool exist = false;
                        int temp = j;
                        for (int k = crosspoint1; k <= crosspoint2; k++)
                        {
                            if (temp == offspring2Vector[k])
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            if (m == crosspoint1)
                            {
                                m = crosspoint2 + 1;
                            }
                            offspring2Vector[m++] = temp;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Cannot perform TwoPointsCrossover ");
            }

            return offspring;
        }

        public override object Execute(object obj)
        {
            var parents = (Solution[]) obj;

            if (parents.Length != 2)
            {
                throw new Exception("operator needs two parents");
            }
            if (
                !(ValidTypes.Contains(parents[0].SolutionType.GetType()) &&
                  ValidTypes.Contains(parents[1].SolutionType.GetType())))
            {
                throw new Exception("the solutions are not of the right type. The type should be 'Permutation', but " +
                                    parents[0].SolutionType.GetType() + " and " +
                                    parents[1].SolutionType.GetType() + " are obtained");
            }

            return DoCrossover(_crossoverProbability, parents[0], parents[1]);
        }
    }
}