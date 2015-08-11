// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Util;
using CSharpMetal.Util.Wrapper;

namespace CSharpMetal.Operators.Crossover
{
    internal class BlxAlphaCrossover : Crossover
    {
        private const double DefaultAlpha = 0.5;

        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (RealSolutionType),
            typeof (ArrayRealSolutionType)
        };

        private readonly double _alpha;
        private readonly double _crossoverProbability;

        public BlxAlphaCrossover(Dictionary<string, object> parameters) : base(parameters)
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

            _alpha = (parameters.TryGetValue("perturbation", out parameter))
                         ? (double) parameter
                         : DefaultAlpha;
        }

        public Solution[] DoCrossover(double probability,
                                      Solution parent1,
                                      Solution parent2)
        {
            Solution[] offSpring = new Solution[2];

            offSpring[0] = new Solution(parent1);
            offSpring[1] = new Solution(parent2);

            XReal x1 = new XReal(parent1);
            XReal x2 = new XReal(parent2);
            XReal offs1 = new XReal(offSpring[0]);
            XReal offs2 = new XReal(offSpring[1]);

            int numberOfVariables = x1.GetNumberOfDecisionVariables();

            if (PseudoRandom.Instance().NextDouble() <= probability)
            {
                int i;
                for (i = 0; i < numberOfVariables; i++)
                {
                    double upperValue = x1.GetUpperBound(i);
                    double lowerValue = x1.GetLowerBound(i);
                    double valueX1 = x1.GetValue(i);
                    double valueX2 = x2.GetValue(i);

                    double max;
                    double min;

                    if (valueX2 > valueX1)
                    {
                        max = valueX2;
                        min = valueX1;
                    }
                    else
                    {
                        max = valueX1;
                        min = valueX2;
                    }

                    double range = max - min;
                    // Ranges of the new alleles ;

                    double minRange = min - range*_alpha;
                    double maxRange = max + range*_alpha;

                    double random = PseudoRandom.Instance().NextDouble();
                    double valueY1 = minRange + random*(maxRange - minRange);

                    random = PseudoRandom.Instance().NextDouble();
                    double valueY2 = minRange + random*(maxRange - minRange);

                    if (valueY1 < lowerValue)
                    {
                        offs1.SetValue(i, lowerValue);
                    }
                    else if (valueY1 > upperValue)
                    {
                        offs1.SetValue(i, upperValue);
                    }
                    else
                    {
                        offs1.SetValue(i, valueY1);
                    }

                    if (valueY2 < lowerValue)
                    {
                        offs2.SetValue(i, lowerValue);
                    }
                    else if (valueY2 > upperValue)
                    {
                        offs2.SetValue(i, upperValue);
                    }
                    else
                    {
                        offs2.SetValue(i, valueY2);
                    }
                }
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
                throw new Exception("the solutions type" + parents[0].SolutionType.GetType() +
                                    "is not allowed with this operator");
            }


            return DoCrossover(_crossoverProbability, parents[0], parents[1]);
        }
    }
}