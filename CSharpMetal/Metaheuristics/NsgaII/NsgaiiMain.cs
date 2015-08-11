// Author : Vandewynckel Julien
// Creation date : 09/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Diagnostics;
using CSharpMetal.Core;
using CSharpMetal.Operators.Crossover;
using CSharpMetal.Operators.Mutation;
using CSharpMetal.Operators.Selection;
using CSharpMetal.Problems.ZDT;
using CSharpMetal.QualityIndicators;

namespace CSharpMetal.Metaheuristics.NsgaII
{
    internal class NsgaiiMain
    {
        private static void Main(string[] args)
        {
            Problem problem; // The problem to solve
            Algorithm algorithm; // The algorithm to use
            Operator crossover; // Crossover operator
            Operator mutation; // Mutation operator
            Operator selection; // Selection operator

            Dictionary<string, Object> parameters; // Operator parameters

            QualityIndicator indicators = null; // Object to get quality indicators


            /*
            if (args.Length == 1) {
      Object [] arrayParameters = {"Real"};
      problem = (new ProblemFactory()).getProblem(args[0],arrayParameters);
    } // if
    else if (args.Length == 2)
    {
      Object [] arrayParameters = {"Real"};
      problem = (new ProblemFactory()).getProblem(args[0], arrayParameters);
      indicators = new QualityIndicator(problem, args[1]) ;
    } // if
    else { // Default problem
      //problem = new Kursawe("Real", 3);
      //problem = new Kursawe("BinaryReal", 3);
      //problem = new Water("Real");
      problem = new ZDT1("ArrayReal", 30);
      //problem = new ConstrEx("Real");
      //problem = new DTLZ1("Real");
      //problem = new OKA2("Real") ;
    } // else
    */
            problem = new ZDT1("ArrayReal", 30);

            algorithm = new Nsgaii(problem);
            //algorithm = new ssNSGAII(problem);

            // Algorithm parameters
            algorithm.InputParameters["populationSize"] = 100;
            algorithm.InputParameters["maxEvaluations"] = 25000;

            // Mutation and Crossover for Real codification 
            parameters = new Dictionary<string, Object>();
            parameters.Add("probability", 0.9);
            parameters.Add("distributionIndex", 20.0);
            crossover = CrossoverFactory.GetCrossoverOperator("SBXCrossover", parameters);

            parameters = new Dictionary<string, Object>();
            parameters.Add("probability", 1.0/problem.NumberOfVariables);
            parameters.Add("distributionIndex", 20.0);
            mutation = MutationFactory.GetMutationOperator("PolynomialMutation", parameters);

            // Selection Operator 
            parameters = null;
            selection = SelectionFactory.GetSelectionOperator("BinaryTournament2", parameters);

            // Add the operators to the algorithm
            algorithm.UsedOperators.Add("crossover", crossover);
            algorithm.UsedOperators.Add("mutation", mutation);
            algorithm.UsedOperators.Add("selection", selection);

            // Add the indicator object to the algorithm
            algorithm.InputParameters.Add("indicators", indicators);

            // Execute the Algorithm
            var watch = Stopwatch.StartNew();
            SolutionSet population = algorithm.Execute();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            // Result messages 
            Console.WriteLine("Total execution time: " + elapsedMs + "ms");
            Console.WriteLine("Variables values have been writen to file VAR");
            population.PrintVariablesToFile("VAR");
            Console.WriteLine("Objectives values have been writen to file FUN");
            population.PrintObjectivesToFile("FUN");

            if (indicators != null)
            {
                Console.WriteLine("Quality indicators");
                Console.WriteLine("Hypervolume: " + indicators.GetHypervolume(population));
                Console.WriteLine("GD         : " + indicators.GetGd(population));
                Console.WriteLine("IGD        : " + indicators.GetIgd(population));
                Console.WriteLine("Spread     : " + indicators.GetSpread(population));
                Console.WriteLine("Epsilon    : " + indicators.GetEpsilon(population));

                int evaluations = (int) algorithm.OutputParameters["evaluations"];
                Console.WriteLine("Speed      : " + evaluations + " evaluations");
            }
        }
    }
}