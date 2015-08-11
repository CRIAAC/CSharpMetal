// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Operators.Selection
{
    public class BinaryTournament : BaseSelection
    {
        public IComparer Comparator { get; set; }

        public BinaryTournament(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object parameter;
            Comparator = parameters.TryGetValue("comparator", out parameter)
                             ? (IComparer) parameter
                             : new DominanceComparator();
        }

        public override object Execute(object obj)
        {
            SolutionSet solutionSet = (SolutionSet) obj;
            Solution solution1 = solutionSet[(PseudoRandom.Instance().Next(0, solutionSet.Size() - 1))];
            Solution solution2 = solutionSet[(PseudoRandom.Instance().Next(0, solutionSet.Size() - 1))];

            if (solutionSet.Size() >= 2)
            {
                while (solution1 == solution2)
                {
                    solution2 = solutionSet[(PseudoRandom.Instance().Next(0, solutionSet.Size() - 1))];
                }
            }

            int flag = Comparator.Compare(solution1, solution2);
            if (flag == -1)
            {
                return solution1;
            }
            if (flag == 1)
            {
                return solution2;
            }
            if (PseudoRandom.Instance().NextDouble() < 0.5)
            {
                return solution1;
            }
            return solution2;
        }
    }
}