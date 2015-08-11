// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Linq;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Encodings.Variables;
using CSharpMetal.Util;

namespace CSharpMetal.Operators.Crossover
{
    internal class SinglePointCrossover : Crossover
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (BinarySolutionType),
            typeof (BinaryRealSolutionType),
            typeof (IntSolutionType)
        };

        private readonly double _crossoverProbability;

        public SinglePointCrossover(Dictionary<string, object> parameters) : base(parameters)
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
            Solution[] offSpring = new Solution[2];
            offSpring[0] = new Solution(parent1);
            offSpring[1] = new Solution(parent2);
            try
            {
                if (PseudoRandom.Instance().NextDouble() < probability)
                {
                    if ((parent1.SolutionType.GetType() == typeof (BinarySolutionType)) ||
                        (parent1.SolutionType.GetType() == typeof (BinaryRealSolutionType)))
                    {
                        //1. Compute the total number of bits
                        int totalNumberOfBits = parent1.DecisionVariables.Sum(t => ((Binary) t).NumberOfBits);

                        //2. Calculate the point to make the crossover
                        int crossoverPoint = PseudoRandom.Instance().Next(0, totalNumberOfBits - 1);

                        //3. Compute the encodings.variable containing the crossoverPoint bit
                        int variable = 0;
                        int acountBits =
                            ((Binary) parent1.DecisionVariables[variable]).NumberOfBits;

                        while (acountBits < (crossoverPoint + 1))
                        {
                            variable++;
                            acountBits +=
                                ((Binary) parent1.DecisionVariables[variable]).NumberOfBits;
                        }

                        //4. Compute the bit into the selected encodings.variable
                        int diff = acountBits - crossoverPoint;
                        int intoVariableCrossoverPoint =
                            ((Binary) parent1.DecisionVariables[variable]).NumberOfBits - diff;

                        //5. Make the crossover into the gene;
                        var offSpring1 = (Binary) parent1.DecisionVariables[variable].Clone();
                        var offSpring2 = (Binary) parent2.DecisionVariables[variable].Clone();

                        for (int i = intoVariableCrossoverPoint;
                             i < offSpring1.NumberOfBits;
                             i++)
                        {
                            bool swap = offSpring1.Bits.Get(i);
                            offSpring1.Bits.Set(i, offSpring2.Bits.Get(i));
                            offSpring2.Bits.Set(i, swap);
                        }

                        offSpring[0].DecisionVariables[variable] = offSpring1;
                        offSpring[1].DecisionVariables[variable] = offSpring2;

                        //6. Apply the crossover to the other variables
                        for (int i = 0; i < variable; i++)
                        {
                            offSpring[0].DecisionVariables[i] =
                                parent2.DecisionVariables[i].Clone();

                            offSpring[1].DecisionVariables[i] =
                                parent1.DecisionVariables[i].Clone();
                        }

                        //7. Decode the results
                        for (int i = 0; i < offSpring[0].DecisionVariables.Length; i++)
                        {
                            ((Binary) offSpring[0].DecisionVariables[i]).Decode();
                            ((Binary) offSpring[1].DecisionVariables[i]).Decode();
                        }
                    } // Binary or BinaryReal
                    else
                    {
                        // Integer representation
                        int crossoverPoint = PseudoRandom.Instance().Next(0, parent1.NumberOfVariables - 1);
                        for (int i = crossoverPoint; i < parent1.NumberOfVariables; i++)
                        {
                            var valueX1 = (int) parent1.DecisionVariables[i].Value;
                            var valueX2 = (int) parent2.DecisionVariables[i].Value;
                            offSpring[0].DecisionVariables[i].Value = (valueX2);
                            offSpring[1].DecisionVariables[i].Value = (valueX1);
                        } // for
                    } // Int representation
                }
            }
            catch (InvalidCastException e1)
            {
                throw new Exception("Cannot perform singlePointCrossover: " + e1);
            }
            return offSpring;
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
                throw new Exception(
                    "the solutions are not of the right type. The type should be 'Binary', or 'Int, but " +
                    parents[0].SolutionType.GetType() + " and " +
                    parents[1].SolutionType.GetType() + " are obtained");
            }

            Solution[] offSpring = DoCrossover(_crossoverProbability, parents[0], parents[1]);
            foreach (Solution t in offSpring)
            {
                t.CrowdingDistance = 0.0;
                t.Rank = 0;
            }

            return offSpring;
        }
    }
}