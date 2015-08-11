// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using System.Collections.Generic;
using CSharpMetal.Core;

namespace CSharpMetal.Operators.Selection
{
    internal class BestSolutionSelection : BaseSelection
    {
        public IComparer Comparator { get; set; }

        public BestSolutionSelection(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object parameter;
            if (parameters.TryGetValue("comparator", out parameter))
            {
                Comparator = (IComparer) parameter;
            }
            else
            {
                throw new Exception("_defaultComparator is null");
            }
        }

        public override object Execute(object obj)
        {
            SolutionSet solutionSet = (SolutionSet) obj;

            if (solutionSet.Size() == 0)
            {
                return null;
            }

            var bestSolution = 0;

            for (int i = 1; i < solutionSet.Size(); i++)
            {
                if (Comparator.Compare(solutionSet[i], solutionSet[bestSolution]) < 0)
                {
                    bestSolution = i;
                }
            }

            return bestSolution;
        }
    }
}