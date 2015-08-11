// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Operators.Selection
{
    internal class RankingAndCrowdingSelection : BaseSelection
    {
        private static readonly IComparer CrowdingComparator = new CrowdingComparator();
        private readonly Problem _problem;

        public RankingAndCrowdingSelection(Dictionary<string, object> parameters) : base(parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object parameter;
            if (parameters.TryGetValue("problem", out parameter))
            {
                _problem = (Problem) parameter;
            }
            else
            {
                throw new Exception("problem not specified");
            }
        }

        public override object Execute(object obj)
        {
            SolutionSet population = (SolutionSet) obj;
            int populationSize = (int) Parameters["populationSize"];
            SolutionSet result = new SolutionSet(populationSize);

            //->Ranking the union
            Ranking ranking = new Ranking(population);

            int remain = populationSize;
            int index = 0;
            SolutionSet front = null;
            population.Clear();

            //-> Obtain the next front
            front = ranking.GetSubfront(index);

            while ((remain > 0) && (remain >= front.Size()))
            {
                //Asign crowding distance to individuals
                Distance.CrowdingDistanceAssignment(front, _problem.NumberOfObjectives);
                //Add the individuals of this front
                for (int k = 0; k < front.Size(); k++)
                {
                    result.Add(front[k]);
                }

                //Decrement remaint
                remain = remain - front.Size();

                //Obtain the next front
                index++;
                if (remain > 0)
                {
                    front = ranking.GetSubfront(index);
                }
            }

            //-> remain is less than front(index).size, insert only the best one
            if (remain > 0)
            {
                // front containt individuals to insert                        
                Distance.CrowdingDistanceAssignment(front, _problem.NumberOfObjectives);
                front.Sort(CrowdingComparator);
                for (int k = 0; k < remain; k++)
                {
                    result.Add(front[k]);
                }
            }

            return result;
        }
    }
}