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
    public class PolynomialMutation : BaseMutation
    {
        public const double EtaMDefault = 20.0;

        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (RealSolutionType),
            typeof (ArrayRealSolutionType)
        };

        private readonly double _distributionIndex;
        private readonly double _mutationProbability;

        public PolynomialMutation(Dictionary<string, object> parameters) : base(parameters)
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

            _distributionIndex = parameters.TryGetValue("probability", out parameter)
                                     ? (double) parameter
                                     : EtaMDefault;
        }

        public override object Execute(object obj)
        {
            Solution solution = (Solution) obj;

            if (!ValidTypes.Contains(solution.SolutionType.GetType()))
            {
                throw new Exception("the solution type " + solution.SolutionType.GetType() +
                                    " is not allowed with this operator");
            }

            DoMutation(_mutationProbability, solution);

            return solution;
        }

        private void DoMutation(double probability, Solution solution)
        {
            XReal x = new XReal(solution);
            for (int var = 0; var < solution.NumberOfVariables; var++)
            {
                if (PseudoRandom.Instance().NextDouble() <= probability)
                {
                    double y = x.GetValue(var);
                    double yl = x.GetLowerBound(var);
                    double yu = x.GetUpperBound(var);
                    double delta1 = (y - yl)/(yu - yl);
                    double delta2 = (yu - y)/(yu - yl);
                    double rnd = PseudoRandom.Instance().NextDouble();
                    double mutPow = 1.0/(EtaMDefault + 1.0);
                    double xy;
                    double deltaq;
                    double val;
                    if (rnd <= 0.5)
                    {
                        xy = 1.0 - delta1;
                        val = 2.0*rnd + (1.0 - 2.0*rnd)*(Math.Pow(xy, (_distributionIndex + 1.0)));
                        deltaq = Math.Pow(val, mutPow) - 1.0;
                    }
                    else
                    {
                        xy = 1.0 - delta2;
                        val = 2.0*(1.0 - rnd) + 2.0*(rnd - 0.5)*(Math.Pow(xy, (_distributionIndex + 1.0)));
                        deltaq = 1.0 - (Math.Pow(val, mutPow));
                    }
                    y = y + deltaq*(yu - yl);
                    if (y < yl)
                    {
                        y = yl;
                    }
                    if (y > yu)
                    {
                        y = yu;
                    }
                    x.SetValue(var, y);
                }
            }
        }
    }
}