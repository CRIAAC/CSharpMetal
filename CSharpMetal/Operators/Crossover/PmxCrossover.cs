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
    internal class PmxCrossover : Crossover
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (PermutationSolutionType)
        };

        private readonly double _crossoverProbability;

        public PmxCrossover(Dictionary<string, object> parameters) : base(parameters)
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

            int permutationLength = ((Permutation) parent1.DecisionVariables[0]).Size;

            var parent1Vector = ((Permutation) parent1.DecisionVariables[0]).Vector;
            var parent2Vector = ((Permutation) parent2.DecisionVariables[0]).Vector;
            var offspring1Vector = ((Permutation) offspring[0].DecisionVariables[0]).Vector;
            var offspring2Vector = ((Permutation) offspring[1].DecisionVariables[0]).Vector;

            if (PseudoRandom.Instance().NextDouble() < probability)
            {
                //      STEP 1: Get two cutting points
                int cuttingPoint1 = PseudoRandom.Instance().Next(0, permutationLength - 1);
                int cuttingPoint2 = PseudoRandom.Instance().Next(0, permutationLength - 1);
                while (cuttingPoint2 == cuttingPoint1)
                {
                    cuttingPoint2 = PseudoRandom.Instance().Next(0, permutationLength - 1);
                }

                if (cuttingPoint1 > cuttingPoint2)
                {
                    int swap = cuttingPoint1;
                    cuttingPoint1 = cuttingPoint2;
                    cuttingPoint2 = swap;
                } // if
                //      STEP 2: Get the subchains to interchange
                var replacement1 = new int[permutationLength];
                var replacement2 = new int[permutationLength];
                for (int i = 0; i < permutationLength; i++)
                {
                    replacement1[i] = replacement2[i] = -1;
                }

                //      STEP 3: Interchange
                for (int i = cuttingPoint1; i <= cuttingPoint2; i++)
                {
                    offspring1Vector[i] = parent2Vector[i];
                    offspring2Vector[i] = parent1Vector[i];

                    replacement1[parent2Vector[i]] = parent1Vector[i];
                    replacement2[parent1Vector[i]] = parent2Vector[i];
                } // for

                //      STEP 4: Repair offsprings
                for (int i = 0; i < permutationLength; i++)
                {
                    if ((i >= cuttingPoint1) && (i <= cuttingPoint2))
                    {
                        continue;
                    }

                    int n1 = parent1Vector[i];
                    int m1 = replacement1[n1];

                    int n2 = parent2Vector[i];
                    int m2 = replacement2[n2];

                    while (m1 != -1)
                    {
                        n1 = m1;
                        m1 = replacement1[m1];
                    } // while
                    while (m2 != -1)
                    {
                        n2 = m2;
                        m2 = replacement2[m2];
                    } // while
                    offspring1Vector[i] = n1;
                    offspring2Vector[i] = n2;
                } // for
            } // if

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