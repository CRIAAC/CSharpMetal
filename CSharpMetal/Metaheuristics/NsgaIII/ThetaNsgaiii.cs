// Author : Vandewynckel Julien
// Creation date : 13/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CSharpMetal.Core;
using CSharpMetal.QualityIndicators;
using CSharpMetal.Util;

namespace CSharpMetal.Metaheuristics.NsgaIII
{
    public class ThetaNsgaiii : Algorithm
    {
        public Solution[] ReferencePoints { get; set; }
        public Solution IdealPoint { get; set; }
        public int K { get; set; }
        public double Theta { get; set; }

        public ThetaNsgaiii(Problem problema) : base(problema)
        {
        }

        public override SolutionSet Execute()
        {
            int populationSize;
            int maxEvaluations;
            QualityIndicator indicators;

            Operator mutationOperator;
            Operator crossoverOperator;
            Operator selectionOperator;


            object parameter;
            if (InputParameters.TryGetValue("populationSize", out parameter))
            {
                populationSize = (int) parameter;
            }
            else
            {
                throw new Exception("populationSize does not exist");
            }

            if (InputParameters.TryGetValue("maxEvaluations", out parameter))
            {
                maxEvaluations = (int) parameter;
            }
            else
            {
                throw new Exception("maxEvaluations does not exist");
            }

            if (InputParameters.TryGetValue("indicators", out parameter))
            {
                indicators = (QualityIndicator) parameter;
            }
            else
            {
                throw new Exception("maxEvaluations does not exist");
            }

            if (InputParameters.TryGetValue("theta", out parameter))
            {
                Theta = (int) parameter;
            }
            else
            {
                throw new Exception("Theta does not exist");
            }

            if (InputParameters.TryGetValue("H", out parameter))
            {
                K = MetalMath.BinomialCoeff(Problema.NumberOfObjectives - 1,
                                            ((int) parameter) + Problema.NumberOfObjectives - 1);
            }
            else
            {
                throw new Exception("H does not exist");
            }

            // Initializing variables
            var population = new SolutionSet(populationSize);
            var evaluations = 0;

            int requiredEvaluations = 0;

            Operator unknownIOperator;
            //Read the operators
            if (UsedOperators.TryGetValue("mutation", out unknownIOperator))
            {
                mutationOperator = unknownIOperator;
            }
            else
            {
                throw new Exception("mutation does not exist");
            }
            if (UsedOperators.TryGetValue("crossover", out unknownIOperator))
            {
                crossoverOperator = unknownIOperator;
            }
            else
            {
                throw new Exception("crossover does not exist");
            }
            if (UsedOperators.TryGetValue("selection", out unknownIOperator))
            {
                selectionOperator = unknownIOperator;
            }
            else
            {
                throw new Exception("selection does not exist");
            }

            K = populationSize;
            ReferencePoints = new Solution[K];

            //Create references points
            GenerateReferencePoint();

            // Create the initial solutionSet
            for (int i = 0; i < populationSize; i++)
            {
                var newSolution = new Solution(Problema);
                Problema.Evaluate(newSolution);
                Problema.EvaluateConstraints(newSolution);
                evaluations++;
                population.Add(newSolution);
            }

            IdealPoint = new Solution(Problema);
            for (var i = 0; i < Problema.NumberOfObjectives; i++)
            {
                IdealPoint.Objective[i] = double.PositiveInfinity;
            }

            ComputeIdealPoint(population);

            ThetaNonDominatedSort ranking = null;

            // Generations 
            SolutionSet union = null;

            while (evaluations < maxEvaluations)
            {
                // Create the offSpring solutionSet      
                var offspringPopulation = new SolutionSet(populationSize);
                Solution[] parents = new Solution[2];

                for (int i = 0; i < (populationSize/2); i++)
                {
                    if (evaluations < maxEvaluations)
                    {
                        //obtain parents
                        parents[0] = (Solution) selectionOperator.Execute(population);
                        parents[1] = (Solution) selectionOperator.Execute(population);
                        Solution[] offSpring = (Solution[]) crossoverOperator.Execute(parents);
                        mutationOperator.Execute(offSpring[0]);
                        mutationOperator.Execute(offSpring[1]);
                        Problema.Evaluate(offSpring[0]);
                        Problema.EvaluateConstraints(offSpring[0]);
                        Problema.Evaluate(offSpring[1]);
                        Problema.EvaluateConstraints(offSpring[1]);
                        offspringPopulation.Add(offSpring[0]);
                        offspringPopulation.Add(offSpring[1]);
                        evaluations += 2;
                    }
                }

                //Update ideal point


                // Create the solutionSet union of solutionSet and offSpring
                union = population.Union(offspringPopulation);
                ComputeIdealPoint(offspringPopulation);

                //Normalize                 
                NormalizePopulation(union);

                //Clustering and non dominated sort
                ranking = new ThetaNonDominatedSort(union, Clustering(union), Theta);

                //Generate next pop
                population = new SolutionSet(populationSize);
                int front = 0;
                while (population.Size() + ranking.GetSubfront(front).Size() <= populationSize)
                {
                    var frontResult = ranking.GetSubfront(front);
                    foreach (var r in frontResult.SolutionList)
                    {
                        population.Add(r);
                    }
                    front++;
                }

                //Generate new pop
                var localList =
                    ranking.GetSubfront(front).SolutionList.OrderBy(item => PseudoRandom.Instance().Next()).ToList();
                int localSizePt1 = population.Size();
                for (int i = 0; i < populationSize - localSizePt1; i++)
                {
                    population.Add(localList[i]);
                }

                if ((indicators != null) && (requiredEvaluations == 0))
                {
                    /*
                    double hv = indicators.GetHypervolume(population);
                    if (hv >= (0.98 * indicators.GetTrueParetoFrontHypervolume()))
                    {
                        requiredEvaluations = evaluations;
                    }*/

                    double gd = indicators.GetGd(ranking.GetSubfront(0));
                    Console.WriteLine(gd);
                }
            }

            OutputParameters["evaluations"] = requiredEvaluations;
            ranking.GetSubfront(0).PrintFeasibleFUN("FUN_THETA_NSGAIII");
            Console.WriteLine("before quitting");
            Console.WriteLine(indicators.GetGd(ranking.GetSubfront(0)));
            Console.WriteLine("before quitting");
            return ranking.GetSubfront(0);
        }

        private List<Cluster> Clustering(SolutionSet normalizedPop)
        {
            List<Cluster> clusters = new List<Cluster>(K);

            for (var c = 0; c < K; c++)
            {
                clusters.Add(new Cluster(ReferencePoints[c], normalizedPop.Size()));
            }

            for (var x = 0; x < normalizedPop.Size(); x++)
            {
                var k = 0;
                var min = Distance(normalizedPop.SolutionList[x], ReferencePoints[k]);
                for (var j = 1; j < K; j++)
                {
                    var d2 = Distance(normalizedPop.SolutionList[x], ReferencePoints[j]);
                    if (d2.Item2 < min.Item2)
                    {
                        min = d2;
                        k = j;
                    }
                }
                clusters[k].Add(normalizedPop.SolutionList[x], min);
            }

            return clusters;
        }

        private static Tuple<double, double> Distance(Solution solution, Solution reference)
        {
            var numerator = 0.0;
            var denominator = 0.0;

            for (var i = 0; i < solution.NumberOfObjectives; i++)
            {
                numerator += Math.Pow(solution.Objective[i]*reference.Objective[i], 2);
                denominator += Math.Pow(reference.Objective[i], 2);
            }

            var normeLambda = Math.Sqrt(denominator);
            var d1 = Math.Sqrt(numerator)/normeLambda;
            numerator = 0;
            for (var i = 0; i < solution.NumberOfObjectives; i++)
            {
                numerator += Math.Pow(solution.Objective[i] - d1*(reference.Objective[i]/normeLambda), 2);
            }

            return new Tuple<double, double>(d1, Math.Sqrt(numerator));
        }

        private void NormalizePopulation(SolutionSet pop)
        {
            Solution z = new Solution(Problema);
            for (var i = 0; i < Problema.NumberOfObjectives; i++)
            {
                z.Objective[i] = double.NegativeInfinity;
            }

            foreach (var x in pop.SolutionList)
            {
                for (var i = 0; i < Problema.NumberOfObjectives; i++)
                {
                    if (x.Objective[i] > z.Objective[i])
                    {
                        z.Objective[i] = x.Objective[i];
                    }
                }
            }

            foreach (var x in pop.SolutionList)
            {
                for (var i = 0; i < Problema.NumberOfObjectives; i++)
                {
                    x.Objective[i] = (x.Objective[i] - IdealPoint.Objective[i])/
                                     (z.Objective[i] - IdealPoint.Objective[i]);
                }
            }
        }

        private void ComputeIdealPoint(SolutionSet pop)
        {
            if (pop.Size() <= 0)
            {
                throw new Exception("Empty pop");
            }

            pop.SolutionList.ForEach(sol =>
                                     {
                                         for (var i = 0; i < sol.NumberOfObjectives; i++)
                                         {
                                             if (sol.Objective[i] < IdealPoint.Objective[i])
                                             {
                                                 IdealPoint.Objective[i] = sol.Objective[i];
                                             }
                                         }
                                     });
        }

        private void GenerateReferencePoint()
        {
            for (var i = 0; i < K; i++)
            {
                double s = 0.0;
                ReferencePoints[i] = new Solution(Problema);
                for (var k = 0; k < Problema.NumberOfObjectives; k++)
                {
                    if (k < Problema.NumberOfObjectives - 1)
                    {
                        var r = PseudoRandom.Instance().NextDouble();
                        ReferencePoints[i].Objective[k] = (1 - s)*
                                                          (1 - Math.Pow(r, 1.0/(Problema.NumberOfObjectives - k)));
                        s += ReferencePoints[i].Objective[k];
                    }
                    else
                    {
                        ReferencePoints[i].Objective[k] = 1 - s;
                    }
                }
            }
        }

        private void printObjectives(string filename, SolutionSet pop)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            TextWriter tw = new StreamWriter(filename);

            pop.SolutionList.ForEach(sol => { tw.WriteLine(string.Join(",", sol.Objective)); });
            tw.Close();
        }
    }
}