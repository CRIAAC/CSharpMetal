// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Util;
using CSharpMetal.Util.Wrapper;

namespace CSharpMetal.Operators.Crossover
{
    public class SbxCrossover : Crossover
    {
        public const double EtaCDefault = 20.0;
        // EPS defines the minimum difference allowed between real values
        private const double Eps = 1.0e-14;

        private static readonly List<Type> ValidTypes = new List<Type>
        {
            typeof (RealSolutionType),
            typeof (ArrayRealSolutionType)
        };

        private readonly double _crossoverProbability = 0.9;
        private readonly double _distributionIndex = EtaCDefault;

        public SbxCrossover(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object parameter;
            _crossoverProbability = parameters.TryGetValue("probability", out parameter) ? (double) parameter : 0.9;
            _distributionIndex = parameters.TryGetValue("distributionIndex", out parameter)
                                     ? (double) parameter
                                     : EtaCDefault;
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
                throw new Exception("the solutions type" + parents[0].SolutionType +
                                    "is not allowed with this operator");
            }


            return DoCrossover(_crossoverProbability, parents[0], parents[1]);
        }

        private Solution[] DoCrossover(double probability, Solution parent1, Solution parent2)
        {
            var offSpring = new Solution[2];

            offSpring[0] = new Solution(parent1);
            offSpring[1] = new Solution(parent2);

            int i;
            double rand;
            double y1, y2, yL, yu;
            double c1, c2;
            double alpha, beta, betaq;
            double valueX1, valueX2;
            XReal x1 = new XReal(parent1);
            XReal x2 = new XReal(parent2);
            XReal offs1 = new XReal(offSpring[0]);
            XReal offs2 = new XReal(offSpring[1]);


            int numberOfVariables = x1.GetNumberOfDecisionVariables();

            if (PseudoRandom.Instance().NextDouble() <= probability)
            {
                for (i = 0; i < numberOfVariables; i++)
                {
                    valueX1 = x1.GetValue(i);
                    valueX2 = x2.GetValue(i);
                    if (PseudoRandom.Instance().NextDouble() <= 0.5)
                    {
                        if (Math.Abs(valueX1 - valueX2) > Eps)
                        {
                            if (valueX1 < valueX2)
                            {
                                y1 = valueX1;
                                y2 = valueX2;
                            }
                            else
                            {
                                y1 = valueX2;
                                y2 = valueX1;
                            } // if                       

                            yL = x1.GetLowerBound(i);
                            yu = x1.GetUpperBound(i);
                            rand = PseudoRandom.Instance().NextDouble();
                            beta = 1.0 + (2.0*(y1 - yL)/(y2 - y1));
                            alpha = 2.0 - Math.Pow(beta, -(_distributionIndex + 1.0));

                            if (rand <= (1.0/alpha))
                            {
                                betaq = Math.Pow((rand*alpha), (1.0/(_distributionIndex + 1.0)));
                            }
                            else
                            {
                                betaq = Math.Pow((1.0/(2.0 - rand*alpha)), (1.0/(_distributionIndex + 1.0)));
                            } // if

                            c1 = 0.5*((y1 + y2) - betaq*(y2 - y1));
                            beta = 1.0 + (2.0*(yu - y2)/(y2 - y1));
                            alpha = 2.0 - Math.Pow(beta, -(_distributionIndex + 1.0));

                            if (rand <= (1.0/alpha))
                            {
                                betaq = Math.Pow((rand*alpha), (1.0/(_distributionIndex + 1.0)));
                            }
                            else
                            {
                                betaq = Math.Pow((1.0/(2.0 - rand*alpha)), (1.0/(_distributionIndex + 1.0)));
                            } // if

                            c2 = 0.5*((y1 + y2) + betaq*(y2 - y1));

                            if (c1 < yL)
                            {
                                c1 = yL;
                            }

                            if (c2 < yL)
                            {
                                c2 = yL;
                            }

                            if (c1 > yu)
                            {
                                c1 = yu;
                            }

                            if (c2 > yu)
                            {
                                c2 = yu;
                            }

                            if (PseudoRandom.Instance().NextDouble() <= 0.5)
                            {
                                offs1.SetValue(i, c2);
                                offs2.SetValue(i, c1);
                            }
                            else
                            {
                                offs1.SetValue(i, c1);
                                offs2.SetValue(i, c2);
                            } // if
                        }
                        else
                        {
                            offs1.SetValue(i, valueX1);
                            offs2.SetValue(i, valueX2);
                        } // if
                    }
                    else
                    {
                        offs1.SetValue(i, valueX2);
                        offs2.SetValue(i, valueX1);
                    } // if
                } // if
            } // if

            return offSpring;
        }
    }
}