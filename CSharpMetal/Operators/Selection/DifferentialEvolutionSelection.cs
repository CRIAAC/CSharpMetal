// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;

namespace CSharpMetal.Operators.Selection
{
    internal class DifferentialEvolutionSelection : BaseSelection
    {
        public DifferentialEvolutionSelection(Dictionary<string, object> parameters) : base(parameters)
        {
        }

        public override object Execute(object obj)
        {
            Object[] parameters = (Object[]) obj;
            SolutionSet population = (SolutionSet) parameters[0];
            int index = (int) parameters[1];

            Solution[] parents = new Solution[3];
            int r1, r2, r3;

            if (population.Size() < 4)
            {
                throw new Exception("DifferentialEvolutionSelection: the population has less than four solutions");
            }

            do
            {
                r1 = PseudoRandom.Instance().Next(0, population.Size() - 1);
            } while (r1 == index);
            do
            {
                r2 = PseudoRandom.Instance().Next(0, population.Size() - 1);
            } while (r2 == index || r2 == r1);
            do
            {
                r3 = PseudoRandom.Instance().Next(0, population.Size() - 1);
            } while (r3 == index || r3 == r1 || r3 == r2);

            parents[0] = population[r1];
            parents[1] = population[r2];
            parents[2] = population[r3];

            return parents;
        }
    }
}