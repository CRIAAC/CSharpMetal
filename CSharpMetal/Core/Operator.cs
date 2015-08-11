// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMetal.Core
{
    /// <summary>
    ///     Uses class
    /// </summary>
    public abstract class Operator
    {
        /// <summary>
        ///     Stores the current operator parameters.
        ///     It is defined as a Map of pairs <<code>String</code>, <code>Object</code>>,
        ///     and it allow objects to be accessed by their names, which  are specified
        ///     by the string.
        /// </summary>
        //private Dictionary<String, Object> _parameters;
        public Dictionary<String, Object> Parameters { get; set; }

        /// <summary>
        ///     Operator name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Operator(Dictionary<string, object> parameters)
        {
            Parameters = parameters;
            Name = "noname";
        }

        /// <summary>
        ///     Abstract method that must be defined by all the operators. When invoked,
        ///     this method executes the operator represented by the current object.
        /// </summary>
        /// <param name="obj">
        ///     This param inherits from Object to allow different kinds
        ///     of parameters for each operator.
        /// </param>
        /// <returns>An object reference. The returned value depens on the operator</returns>
        public abstract Object Execute(Object obj);

        public override string ToString()
        {
            return Parameters.Aggregate("", (current, kvp) => current + (kvp.Key + " = " + kvp.Value + "\n"));
        }
    }
}