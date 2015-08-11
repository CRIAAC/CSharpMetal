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
    internal class HuxCrossover : Crossover
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (BinarySolutionType),
            typeof (BinaryRealSolutionType)
        };

        private readonly double _crossoverProbability;

        public HuxCrossover(Dictionary<string, object> parameters) : base(parameters)
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
                    for (int var = 0; var < parent1.DecisionVariables.Length; var++)
                    {
                        Binary p1 = (Binary) parent1.DecisionVariables[var];
                        Binary p2 = (Binary) parent2.DecisionVariables[var];

                        for (int bit = 0; bit < p1.NumberOfBits; bit++)
                        {
                            if (p1.Bits.Get(bit) != p2.Bits.Get(bit))
                            {
                                if (PseudoRandom.Instance().NextDouble() < 0.5)
                                {
                                    ((Binary) offSpring[0].DecisionVariables[var])
                                        .Bits.Set(bit, p2.Bits.Get(bit));
                                    ((Binary) offSpring[1].DecisionVariables[var])
                                        .Bits.Set(bit, p1.Bits.Get(bit));
                                }
                            }
                        }
                    }
                    //7. Decode the results
                    for (int i = 0; i < offSpring[0].DecisionVariables.Length; i++)
                    {
                        ((Binary) offSpring[0].DecisionVariables[i]).Decode();
                        ((Binary) offSpring[1].DecisionVariables[i]).Decode();
                    }
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
                    "the solutions are not of the right type. The type should be 'Binary' of 'BinaryReal', but" +
                    parents[0].SolutionType.GetType() +
                    "and " + parents[1].SolutionType.GetType() +
                    "are obtained");
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