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
    internal class NonUniformMutation : BaseMutation
    {
        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (RealSolutionType),
            typeof (ArrayRealSolutionType)
        };

        private readonly double _maxIterations;
        private readonly double _mutationProbability;
        private readonly double _perturbation;
        private double _currentIteration = Double.NaN;

        public NonUniformMutation(Dictionary<string, object> parameters) : base(parameters)
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

            if (parameters.TryGetValue("probability", out parameter))
            {
                _perturbation = (double) parameter;
            }
            else
            {
                throw new Exception("perturbation_ is a NaN");
            }

            if (parameters.TryGetValue("distributionIndex", out parameter))
            {
                _maxIterations = (double) parameter;
            }
            else
            {
                throw new Exception("maxIterations_ is a NaN");
            }
        }

        public void DoMutation(double probability, Solution solution)
        {
            XReal x = new XReal(solution);
            for (int var = 0; var < solution.DecisionVariables.Length; var++)
            {
                if (PseudoRandom.Instance().NextDouble() < probability)
                {
                    double rand = PseudoRandom.Instance().NextDouble();
                    double tmp;

                    if (rand <= 0.5)
                    {
                        tmp = Delta(x.GetUpperBound(var) - x.GetValue(var),
                                    _perturbation);
                        tmp += x.GetValue(var);
                    }
                    else
                    {
                        tmp = Delta(x.GetLowerBound(var) - x.GetValue(var),
                                    _perturbation);
                        tmp += x.GetValue(var);
                    }

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

        private double Delta(double y, double bMutationParameter)
        {
            double rand = PseudoRandom.Instance().NextDouble();

            return (y*(1.0 -
                       Math.Pow(rand,
                                Math.Pow((1.0 - _currentIteration/_maxIterations), bMutationParameter)
                           )));
        }

        public override object Execute(object obj)
        {
            Solution solution = (Solution) obj;

            if (!ValidTypes.Contains(solution.SolutionType.GetType()))
            {
                throw new Exception("the solution " + solution.SolutionType.GetType() + " is not of the right type");
            }
            object parameter;
            if (Parameters.TryGetValue("currentIteration", out parameter))
            {
                _currentIteration = (double) parameter;
            }

            DoMutation(_mutationProbability, solution);

            return solution;
        }
    }
}