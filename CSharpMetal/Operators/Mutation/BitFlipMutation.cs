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
    internal class BitFlipMutation : BaseMutation
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (BinarySolutionType),
            typeof (BinaryRealSolutionType),
            typeof (IntSolutionType)
        };

        private readonly double _mutationProbability;

        public BitFlipMutation(Dictionary<string, object> parameters) : base(parameters)
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
            if ((solution.SolutionType.GetType() == typeof (BinarySolutionType)) ||
                (solution.SolutionType.GetType() == typeof (BinaryRealSolutionType)))
            {
                for (int i = 0; i < solution.DecisionVariables.Length; i++)
                {
                    for (int j = 0; j < ((Binary) solution.DecisionVariables[i]).NumberOfBits; j++)
                    {
                        if (PseudoRandom.Instance().NextDouble() < probability)
                        {
                            ((Binary) solution.DecisionVariables[i]).Bits.Flip(j);
                        }
                    }
                }

                for (int i = 0; i < solution.DecisionVariables.Length; i++)
                {
                    ((Binary) solution.DecisionVariables[i]).Decode();
                }
            } // if
            if (solution.SolutionType.GetType() == typeof (IntSolutionType))
            {
                // Integer representation
                for (int i = 0; i < solution.DecisionVariables.Length; i++)
                {
                    if (PseudoRandom.Instance().NextDouble() < probability)
                    {
                        int value = PseudoRandom.Instance().Next(
                            (int) solution.DecisionVariables[i].LowerBound,
                            (int) solution.DecisionVariables[i].UpperBound);
                        solution.DecisionVariables[i].Value = value;
                    } // if
                }
            }
            else
            {
                throw new Exception("");
            }
        }

        public override object Execute(object obj)
        {
            throw new NotImplementedException();
        }
    }
}