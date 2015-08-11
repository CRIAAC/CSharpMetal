// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;

namespace CSharpMetal.Operators.Selection
{
    internal class RandomSelection : BaseSelection
    {
        public RandomSelection(Dictionary<string, object> parameters) : base(parameters)
        {
        }

        public override object Execute(object obj)
        {
            SolutionSet population = (SolutionSet) obj;
            int pos1 = PseudoRandom.Instance().Next(0, population.Size() - 1);
            int pos2 = PseudoRandom.Instance().Next(0, population.Size() - 1);
            while ((pos1 == pos2) && (population.Size() > 1))
            {
                pos2 = PseudoRandom.Instance().Next(0, population.Size() - 1);
            }

            Solution[] parents = new Solution[2];
            parents[0] = population[pos1];
            parents[1] = population[pos2];

            return parents;
        }
    }
}