// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Linq;
using CSharpMetal.Util.Clonable;

namespace CSharpMetal.Core
{
    public class Solution : ITCloneable<Solution>
    {
        // Problema to solve
        private readonly Problem _problem;
        public int Location { get; set; }
        public double Fitness { get; set; }
        public double KDistance { get; set; }
        public double DistanceToSolutionSet { get; set; }
        // Solution type
        public BaseSolutionType SolutionType { get; set; }
        // Decision variables of the solution.
        //private Variable[] variable;
        public BaseVariable[] DecisionVariables { get; set; }
        // Objectives values of the solution.
        //private double[] objective;
        public double[] Objective { get; set; }
        // Number of objective values of the solution
        public int NumberOfObjectives { get; set; }
        // Number of variables of the solution
        public int NumberOfVariables
        {
            get { return _problem.NumberOfVariables; }
        }

        public bool Marked { get; set; }
        // Rank of the solution. Used in NSGA-II
        public int Rank { get; set; }
        // Overall constraint violation value of the solution
        public double OverallConstraintViolation { get; set; }
        // Number of constraints violated by the solution
        public int NumberOfViolatedConstraints { get; set; }
        // Crowding distance of the the solution in a SolutionSet. Used in NSGA-II.
        public double CrowdingDistance { get; set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Solution()
        {
            _problem = null;
            Marked = false;
            OverallConstraintViolation = 0.0;
            NumberOfViolatedConstraints = 0;
            SolutionType = null;
            DecisionVariables = null;
            Objective = null;
        }

        // Solution

        public Solution(int numberOfObjectives)
        {
            NumberOfObjectives = numberOfObjectives;
            Objective = new double[numberOfObjectives];
        }

        public Solution(Problem problem)
        {
            _problem = problem;
            SolutionType = _problem.TypeOfSolution;
            NumberOfObjectives = _problem.NumberOfObjectives;
            Objective = new double[NumberOfObjectives];

            // Setting initial values
            Fitness = 0.0;
            KDistance = 0.0;
            CrowdingDistance = 0.0;
            DistanceToSolutionSet = double.PositiveInfinity;
            DecisionVariables = SolutionType.CreateVariables();
        }

        public Solution(Problem problem, BaseVariable[] variables)
        {
            _problem = problem;
            SolutionType = problem.TypeOfSolution;
            NumberOfObjectives = _problem.NumberOfObjectives;
            Objective = new double[NumberOfObjectives];

            // Setting initial values
            Fitness = 0.0;
            KDistance = 0.0;
            CrowdingDistance = 0.0;
            DistanceToSolutionSet = double.PositiveInfinity;
            DecisionVariables = SolutionType.CreateVariables();
        }

        // Solution

        /// <summary>
        ///     Copy constructor
        /// </summary>
        /// <param name="solution">
        ///     A <see cref="Solution" />
        /// </param>
        public Solution(Solution solution)
        {
            _problem = solution._problem;
            SolutionType = solution.SolutionType;

            NumberOfObjectives = solution.NumberOfObjectives;
            Objective = new double[NumberOfObjectives];

            Array.Copy(solution.Objective, Objective, NumberOfObjectives);

            DecisionVariables = SolutionType.CopyVariables(solution.DecisionVariables);
            OverallConstraintViolation = solution.OverallConstraintViolation;
            NumberOfViolatedConstraints = solution.NumberOfViolatedConstraints;
            DistanceToSolutionSet = solution.DistanceToSolutionSet;
            CrowdingDistance = solution.CrowdingDistance;
            KDistance = solution.KDistance;
            Fitness = solution.Fitness;
            Marked = solution.Marked;
            Rank = solution.Rank;
            Location = solution.Location;
        }

        // Solution
        /*
        public Solution Clone()
        {
            return new Solution(this);
        }*/

        public Solution Clone()
        {
            return new Solution(this);
        }

        public override string ToString()
        {
            return string.Format("[Solution: type={0}, variable={1}, objective={2}]",
                                 string.Join(",", SolutionType),
                                 string.Join<BaseVariable>(",", DecisionVariables),
                                 string.Join(",", Objective)
                );
        }

        public double GetAggregativeValue()
        {
            return Objective.Sum();
        }
    }
}