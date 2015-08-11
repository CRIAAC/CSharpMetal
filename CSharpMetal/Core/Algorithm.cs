// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;

namespace CSharpMetal.Core
{
    /// <summary>
    ///     Abstract lass presenting an algorithm
    /// </summary>
    public abstract class Algorithm
    {
        public Problem Problema { get; set; }

        /// <summary>
        ///     Stores the operators used by the algorithm, such as selection, crossover, etc.
        /// </summary>
        public Dictionary<string, Operator> UsedOperators { get; protected set; }

        /// <summary>
        ///     Stores algorithm specific parameters
        /// </summary>
        public Dictionary<String, Object> InputParameters { get; protected set; }

        /// <summary>
        ///     Stores output parameters, returned by the algorithm
        /// </summary>
        public Dictionary<String, Object> OutputParameters { get; protected set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="problema">
        ///     A <see cref="Core.Problem" />
        /// </param>
        public Algorithm(Problem problema) : this()
        {
            Problema = problema;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Algorithm()
        {
            UsedOperators = new Dictionary<string, Operator>();
            InputParameters = new Dictionary<string, Object>();
            OutputParameters = new Dictionary<string, Object>();
        }

        /// <summary>
        ///     Abstract method to execute the algorithm
        /// </summary>
        /// <returns></returns>
        public abstract SolutionSet Execute();
    }
}