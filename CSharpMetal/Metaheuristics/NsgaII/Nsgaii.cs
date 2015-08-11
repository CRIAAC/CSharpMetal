// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.QualityIndicators;
using CSharpMetal.Util;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Metaheuristics.NsgaII
{
    public class Nsgaii : Algorithm
    {
        public Nsgaii(Problem problema) : base(problema)
        {
        }

        // NSGAII
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

            // Create the initial solutionSet
            for (int i = 0; i < populationSize; i++)
            {
                var newSolution = new Solution(Problema);
                Problema.Evaluate(newSolution);
                Problema.EvaluateConstraints(newSolution);
                evaluations++;
                population.Add(newSolution);
            } //for       

            // Generations 
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
                    } // if                            
                } // for

                System.Console.WriteLine("evaluation #: " + evaluations);

                // Create the solutionSet union of solutionSet and offSpring
                SolutionSet union = population.Union(offspringPopulation);

                // Ranking the union
                Ranking localRanking = new Ranking(union);

                int remain = populationSize;
                int index = 0;
                SolutionSet front = null;
                population.Clear();

                // Obtain the next front
                front = localRanking.GetSubfront(index);

                while ((remain > 0) && (remain >= front.Size()))
                {
                    //Assign crowding distance to individuals
                    Distance.CrowdingDistanceAssignment(front, Problema.NumberOfObjectives);
                    //Add the individuals of this front
                    for (int k = 0; k < front.Size(); k++)
                    {
                        population.Add(front[k]);
                    } // for

                    //Decrement remain
                    remain = remain - front.Size();

                    //Obtain the next front
                    index++;
                    if (remain > 0)
                    {
                        front = localRanking.GetSubfront(index);
                    } // if        
                } // while

                // Remain is less than front(index).size, insert only the best one
                if (remain > 0)
                {
                    // front contains individuals to insert                        
                    Distance.CrowdingDistanceAssignment(front, Problema.NumberOfObjectives);
                    front.Sort(new CrowdingComparator());
                    for (int k = 0; k < remain; k++)
                    {
                        population.Add(front[k]);
                    } // for

                    remain = 0;
                } // if                               

                // This piece of code shows how to use the indicator object into the code
                // of NSGA-II. In particular, it finds the number of evaluations required
                // by the algorithm to obtain a Pareto front with a hypervolume higher
                // than the hypervolume of the true Pareto front.
                if ((indicators != null) &&
                    (requiredEvaluations == 0))
                {
                    double hv = indicators.GetHypervolume(population);
                    if (hv >= (0.98*indicators.GetTrueParetoFrontHypervolume()))
                    {
                        requiredEvaluations = evaluations;
                    } // if
                } // if
            } // while

            // Return as output parameter the required evaluations
            OutputParameters["evaluations"] = requiredEvaluations;

            // Return the first non-dominated front
            Ranking ranking = new Ranking(population);
            ranking.GetSubfront(0).PrintFeasibleFUN("FUN_NSGAII");

            return ranking.GetSubfront(0);
        }
    }
}