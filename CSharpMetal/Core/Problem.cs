// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;

namespace CSharpMetal.Core
{
    public abstract class Problem
    {
        // Defines the default precision of binary-coded variables
        private const int DefaultPrecision = 20;
        /*
         * Stores the type of each variable
         */
        public Type[] VariableType;
        /*
     * Stores the length of each variable when applicable (e.g., Binary and
     * Permutation variables)
     */
        protected int[] VarLength;
        /*
     * Stores the number of bits used by binary-coded variables (e.g., BinaryReal
     * variables). By default, they are initialized to DEFAULT_PRECISION)
     */
        public int[] Precision { get; set; }
        // Stores the number of variables of the problem
        public int NumberOfVariables { get; set; }
        // Stores the number of objectives of the problem
        public int NumberOfObjectives { get; set; }
        // Stores the number of constraints of the problem
        public int NumberOfConstraints { get; set; }
        // Stores the problem name
        public string ProblemName { get; set; }
        // Stores the type of the solutions of the problem
        //protected SolutionType solutionType_;
        public BaseSolutionType TypeOfSolution { get; set; }
        // Stores the lower bound values for each variable (only if needed)
        //protected double[] lowerLimit_;
        public double[] LowerLimit { get; set; }
        // Stores the upper bound values for each variable (only if needed)
        //protected double[] upperLimit_;
        public double[] UpperLimit { get; set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Problem()
        {
            TypeOfSolution = null;
            ProblemName = "no name";
        }

        /*
        public Problem Clone()
        {
            var copy = (Problem) MemberwiseClone();
            copy.VarLength = new int[VarLength.Length];
            Array.Copy(VarLength, copy.VarLength, VarLength.Length);

            copy.Precision = new int[Precision.Length];
            Array.Copy(Precision, copy.Precision, Precision.Length);

            copy.VariableType = new SolutionType[VariableType.Length];
            Array.Copy(VariableType, copy.VariableType, VariableType.Length);

            copy.NumberOfVariables = NumberOfVariables;
            copy.NumberOfObjectives = NumberOfObjectives;
            copy.NumberOfConstraints = NumberOfConstraints;
            copy.ProblemName = String.Copy(ProblemName);
            copy.TypeOfSolution = TypeOfSolution.Clone();

            Array.Copy(LowerLimit, copy.LowerLimit, LowerLimit.Length);
            Array.Copy(UpperLimit, copy.UpperLimit, UpperLimit.Length);

            return copy;
        }*/

        public int GetLength(int var)
        {
            if (VarLength == null)
            {
                return DefaultPrecision;
            }
            return VarLength[var];
        }

        // Solves
        public abstract void Evaluate(Solution solution);

        public virtual void EvaluateConstraints(Solution solution)
        {
        }

        public int GetNumberOfBits()
        {
            var result = 0;
            for (var var = 0; var < NumberOfVariables; var++)
            {
                result += GetLength(var);
            }
            return result;
        }
    }
}