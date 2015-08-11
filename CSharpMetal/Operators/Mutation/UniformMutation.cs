// Author : Vandewynckel Julien
// Creation date : 09/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Util;
using CSharpMetal.Util.Wrapper;

namespace CSharpMetal.Operators.Mutation
{
    internal class UniformMutation : BaseMutation
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (RealSolutionType),
            typeof (ArrayRealSolutionType)
        };

        private readonly double _mutationProbability;
        private readonly double _perturbation;

        public UniformMutation(Dictionary<string, object> parameters) : base(parameters)
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
            if (parameters.TryGetValue("perturbation", out parameter))
            {
                _perturbation = (double) parameter;
            }
            else
            {
                throw new Exception("perturbation_ is a NaN");
            }
        }

        public override object Execute(object obj)
        {
            Solution solution = (Solution) obj;

            if (!ValidTypes.Contains(solution.SolutionType.GetType()))
            {
                throw new Exception("the solution type " + solution.SolutionType.GetType() +
                                    " is not of the right type. The type should be 'Real'," +
                                    ", but " + solution.SolutionType.GetType() + " is obtained");
            }

            DoMutation(_mutationProbability, solution);

            return solution;
        }

        public void DoMutation(double probability, Solution solution)
        {
            XReal x = new XReal(solution);

            for (int var = 0; var < solution.DecisionVariables.Length; var++)
            {
                if (PseudoRandom.Instance().NextDouble() < probability)
                {
                    double rand = PseudoRandom.Instance().NextDouble();
                    double tmp = (rand - 0.5)*_perturbation;

                    tmp += x.GetValue(var);

                    if (tmp < x.GetLowerBound(var))
                    {
                        tmp = x.GetLowerBound(var);
                    }
                    else if (tmp > x.GetUpperBound(var))
                    {
                        tmp = x.GetUpperBound(var);
                    }

                    x.SetValue(var, tmp);
                }
            }
        }
    }
}