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
    public class DifferentialEvolutionCrossover : Crossover
    {
        private const double DefaultCr = 0.5;
        private const double DefaultF = 0.5;
        private const double DefaultK = 0.5;
        private const string DefaultDeVariant = "rand/1/bin";
        private static readonly Type[] ValidTypes = {typeof (RealSolutionType), typeof (ArrayRealSolutionType)};
        public double Cr { get; set; }
        public double F { get; set; }
        public double K { get; set; }
        public string DeVariant { get; set; }

        public DifferentialEvolutionCrossover(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            object parameter;
            Cr = parameters.TryGetValue("CR", out parameter) ? (double) parameter : DefaultCr;
            F = parameters.TryGetValue("F", out parameter) ? (double) parameter : DefaultF;
            K = parameters.TryGetValue("K", out parameter) ? (double) parameter : DefaultK;
            DeVariant = parameters.TryGetValue("DE_VARIANT", out parameter) ? (string) parameter : DefaultDeVariant;
        }

        public override Object Execute(Object obj)
        {
            var parameters = (Object[]) obj;
            var current = (Solution) parameters[0];
            var parent = parameters[1] as Solution[];

            if (parent == null)
            {
                throw new Exception("parents in passed object do not exist");
            }

            if ((Array.Find(ValidTypes, n => n == parent[0].SolutionType.GetType()) == null) ||
                (Array.Find(ValidTypes, n => n == parent[1].SolutionType.GetType()) == null) ||
                (Array.Find(ValidTypes, n => n == parent[2].SolutionType.GetType()) == null))
            {
                throw new Exception("the solutions are not of the right type. The type should be 'Real' or 'ArrayReal', but"
                                    + parent[0].SolutionType.GetType() + "and"
                                    + parent[1].SolutionType.GetType() + "and"
                                    + parent[2].SolutionType.GetType() + "are obtained");
            }

            int numberOfVariables = current.NumberOfVariables;
            int jrand = PseudoRandom.Instance().Next(numberOfVariables - 1);

            var child = new Solution(current);

            XReal xParent0 = new XReal(parent[0]);
            XReal xParent1 = new XReal(parent[1]);
            XReal xParent2 = new XReal(parent[2]);
            XReal xCurrent = new XReal(current);
            XReal xChild = new XReal(child);

            switch (DeVariant)
            {
                case "rand/1/bin":
                case "best/1/bin":

                    for (var j = 0; j < numberOfVariables; j++)
                    {
                        if (PseudoRandom.Instance().NextDouble() < Cr || j == jrand)
                        {
                            double value = xParent2.GetValue(j) + F*(xParent0.GetValue(j) -
                                                                     xParent1.GetValue(j));

                            if (value < xChild.GetLowerBound(j))
                            {
                                value = xChild.GetLowerBound(j);
                            }
                            if (value > xChild.GetUpperBound(j))
                            {
                                value = xChild.GetUpperBound(j);
                            }

                            xChild.SetValue(j, value);
                        }
                        else
                        {
                            double value = xCurrent.GetValue(j);
                            xChild.SetValue(j, value);
                        }
                    }

                    break;
                case "rand/1/exp":
                case "best/1/exp":

                    for (var j = 0; j < numberOfVariables; j++)
                    {
                        if (PseudoRandom.Instance().NextDouble() < Cr || j == jrand)
                        {
                            double value = xParent2.GetValue(j) + F*(xParent0.GetValue(j) -
                                                                     xParent1.GetValue(j));

                            if (value < xChild.GetLowerBound(j))
                            {
                                value = xChild.GetLowerBound(j);
                            }
                            if (value > xChild.GetUpperBound(j))
                            {
                                value = xChild.GetUpperBound(j);
                            }

                            xChild.SetValue(j, value);
                        }
                        else
                        {
                            Cr = 0.0;
                            double value = xCurrent.GetValue(j);
                            xChild.SetValue(j, value);
                        }
                    }

                    break;

                case "current-to-rand/1":
                case "current-to-best/1":

                    for (var j = 0; j < numberOfVariables; j++)
                    {
                        if (PseudoRandom.Instance().NextDouble() < Cr || j == jrand)
                        {
                            double value = xCurrent.GetValue(j) + K*(xParent2.GetValue(j) -
                                                                     xCurrent.GetValue(j)) +
                                           F*(xParent0.GetValue(j) - xParent1.GetValue(j));

                            if (value < xChild.GetLowerBound(j))
                            {
                                value = xChild.GetLowerBound(j);
                            }
                            if (value > xChild.GetUpperBound(j))
                            {
                                value = xChild.GetUpperBound(j);
                            }

                            xChild.SetValue(j, value);
                        }
                    }

                    break;

                case "current-to-rand/1/bin":
                case "current-to-best/1/bin":

                    for (var j = 0; j < numberOfVariables; j++)
                    {
                        if (PseudoRandom.Instance().NextDouble() < Cr || j == jrand)
                        {
                            double value = xCurrent.GetValue(j) + K*(xParent2.GetValue(j) -
                                                                     xCurrent.GetValue(j)) +
                                           F*(xParent0.GetValue(j) - xParent1.GetValue(j));

                            if (value < xChild.GetLowerBound(j))
                            {
                                value = xChild.GetLowerBound(j);
                            }
                            if (value > xChild.GetUpperBound(j))
                            {
                                value = xChild.GetUpperBound(j);
                            }

                            xChild.SetValue(j, value);
                        }
                        else
                        {
                            Cr = 0.0;
                            double value = xCurrent.GetValue(j);
                            xChild.SetValue(j, value);
                        }
                    }

                    break;

                default:
                    throw new Exception("Unknown DE variant (" + DeVariant + ")");
            }

            return child;
        }
    }
}