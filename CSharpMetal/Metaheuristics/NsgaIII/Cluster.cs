// Author : Vandewynckel Julien
// Creation date : 13/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using CSharpMetal.Core;

namespace CSharpMetal.Metaheuristics.NsgaIII
{
    public class Cluster
    {
        public SolutionSet Solutions { get; private set; }
        public List<Tuple<double, double>> Distances { get; private set; }
        public Solution ReferencePoint { get; set; }

        public int Count
        {
            get { return Solutions.Size(); }
        }

        public Tuple<Solution, Tuple<double, double>> this[int index]
        {
            get { return new Tuple<Solution, Tuple<double, double>>(Solutions[index], Distances[index]); }
            set
            {
                Solutions[index] = value.Item1;
                Distances[index] = value.Item2;
            }
        }

        public Cluster(Solution referencePoint, int populationSize)
        {
            Solutions = new SolutionSet(populationSize);
            Distances = new List<Tuple<double, double>>();
            ReferencePoint = referencePoint.Clone();
        }

        public void Add(Solution solution, Tuple<double, double> distances)
        {
            if (solution != null && distances != null)
            {
                Solutions.Add(solution);
                Distances.Add(distances);
            }
        }
    }
}