// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Operators.Selection
{
    internal class BinaryTournament2 : BaseSelection
    {
        private int[] _a;
        private int _index;
        public IComparer Dominance { get; set; }

        public BinaryTournament2(Dictionary<string, object> parameters) : base(parameters)
        {
            Dominance = new DominanceComparator();
        }

        public override object Execute(object obj)
        {
            SolutionSet population = (SolutionSet) obj;
            if (_index == 0) //Create the permutation
            {
                _a = PermutationUtility.IntPermutation(population.Size());
            }


            Solution solution1 = population[(_a[_index])];
            Solution solution2 = population[(_a[_index + 1])];

            _index = (_index + 2)%population.Size();

            int flag = Dominance.Compare(solution1, solution2);
            if (flag == -1)
            {
                return solution1;
            }
            if (flag == 1)
            {
                return solution2;
            }
            if (solution1.CrowdingDistance > solution2.CrowdingDistance)
            {
                return solution1;
            }
            if (solution2.CrowdingDistance > solution1.CrowdingDistance)
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