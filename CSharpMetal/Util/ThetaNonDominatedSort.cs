// Author : Vandewynckel Julien
// Creation date : 13/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpMetal.Core;
using CSharpMetal.Metaheuristics.NsgaIII;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Util
{
    public class ThetaNonDominatedSort
    {
        protected readonly List<SolutionSet> _ranking;
        protected readonly SolutionSet _solutionSet;
        protected readonly IComparer Constraint = new OverallConstraintViolationComparator();
        protected readonly IComparer Dominance;

        public ThetaNonDominatedSort(SolutionSet population, List<Cluster> clusters, double theta)
        {
            Dominance = new ThetaComparator(theta);
            _solutionSet = population;
            _ranking = new List<SolutionSet>();
            // dominateMe[i] contains the number of solutions dominating i        
            int[] dominateMe = new int[_solutionSet.Size()];

            // iDominate[k] contains the list of solutions dominated by k
            List<int>[] iDominate = new List<int>[_solutionSet.Size()];

            // front[i] contains the list of individuals belonging to the front i
            List<int>[] front = new List<int>[_solutionSet.Size() + 1];

            // flagDominate is an auxiliar encodings.variable
            int flagDominate;

            for (int p = 0; p < (clusters.Count - 1); p++)
            {
                // Initialize the fronts 
                for (int k = 0; k < front.Length; k++)
                {
                    front[k] = new List<int>();
                }
                for (int q = 0; q < clusters[p].Count; q++)
                {
                    // Initialize the list of individuals that i dominate and the number
                    // of individuals that dominate me
                    iDominate[q] = new List<int>();
                    dominateMe[q] = 0;
                }

                for (int q = 0; q < (clusters[p].Count - 1); q++)
                {
                    for (int r = q + 1; r < (clusters[p].Count - 1); r++)
                    {
                        flagDominate = Dominance.Compare(clusters[p][q].Item2, clusters[p][r].Item2);
                        //flagDominate = Constraint.Compare(clusters[p][q].Item1, clusters[p][r].Item1);
                        if (flagDominate == 0)
                        {
                            flagDominate = Dominance.Compare(clusters[p][q].Item2, clusters[p][r].Item2);
                        }
                        if (flagDominate == -1)
                        {
                            iDominate[q].Add(r);
                            dominateMe[r]++;
                        }
                        else if (flagDominate == 1)
                        {
                            iDominate[r].Add(q);
                            dominateMe[q]++;
                        }
                    }
                }

                //Fill front
                if (_ranking.Count == 0)
                {
                    _ranking.Add(new SolutionSet(population.Size()));
                }
                for (int q = 0; q < clusters[p].Count; q++)
                {
                    if (dominateMe[q] == 0)
                    {
                        front[0].Add(q);
                        _ranking[0].Add(clusters[p][q].Item1);
                        clusters[p][q].Item1.Rank = 0;
                    }
                }

                //Obtain the rest of fronts
                int i = 0;
                IEnumerator<int> it1; // Iterators
                while (front[i].Count != 0)
                {
                    i++;
                    if (i >= _ranking.Count)
                    {
                        _ranking.Add(new SolutionSet(population.Size()));
                    }
                    it1 = front[i - 1].GetEnumerator();
                    while (it1.MoveNext())
                    {
                        IEnumerator<int> it2 = iDominate[it1.Current].GetEnumerator(); // Iterators
                        while (it2.MoveNext())
                        {
                            int index = it2.Current;
                            dominateMe[index]--;
                            if (dominateMe[index] == 0)
                            {
                                front[i].Add(index);
                                _ranking[i].Add(clusters[p][index].Item1);
                                clusters[p][index].Item1.Rank = i;
                            }
                        }
                    }
                }
            }
        }

        public SolutionSet GetSubfront(int rank)
        {
            // <pex>
            if ((uint) rank >= (uint) (_ranking.Count))
            {
                var sum = _ranking.Sum(sol => { return sol.SolutionList.Count; });
                Console.WriteLine(sum);
                throw new ArgumentException("complex reason");
            }
            // </pex>
            return _ranking[rank];
        }

        public override string ToString()
        {
            string str = "POPULATION TO RANK (" + _solutionSet.Size() + ")\n";

            for (var i = 0; i < _solutionSet.Size(); i++)
            {
                str += "" + i + ": " + _solutionSet[i] + "\n";
            }

            int l = _ranking.Count;
            str += "Number of ranks: " + l + "\n";

            for (var rank = 0; rank < l; rank++)
            {
                str += "-- Rank: " + rank + "\n";
                for (var sol = 0; sol < _ranking[rank].Size(); sol++)
                {
                    str += _ranking[rank][sol] + "\n";
                }
            }

            return str;
        }

        public int GetNumberOfSubfronts()
        {
            return _ranking.Count;
        }
    }
}