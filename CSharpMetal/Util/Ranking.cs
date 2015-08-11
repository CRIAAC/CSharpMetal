// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Util
{
    public class Ranking
    {
        /**
   * stores a <code>Comparator</code> for dominance checking
   */
        protected static readonly IComparer Dominance = new DominanceComparator();
        /**
   * stores a <code>Comparator</code> for Overal Constraint Violation Comparator
   * checking
   */
        protected static readonly IComparer Constraint = new OverallConstraintViolationComparator();
        /**
   * An array containing all the fronts found during the search
   */
        protected readonly SolutionSet[] _ranking;
        /**
   * The <code>SolutionSet</code> to rank
   */
        protected readonly SolutionSet _solutionSet;

        public Ranking(SolutionSet solutionSet)
        {
            _solutionSet = solutionSet;

            // dominateMe[i] contains the number of solutions dominating i        
            int[] dominateMe = new int[_solutionSet.Size()];

            // iDominate[k] contains the list of solutions dominated by k
            List<int>[] iDominate = new List<int>[_solutionSet.Size()];

            // front[i] contains the list of individuals belonging to the front i
            List<int>[] front = new List<int>[_solutionSet.Size() + 1];

            // flagDominate is an auxiliar encodings.variable
            int flagDominate;

            // Initialize the fronts 
            for (int k = 0; k < front.Length; k++)
            {
                front[k] = new List<int>();
            }

            //-> Fast non dominated sorting algorithm
            // Contribution of Guillaume Jacquenot
            for (int p = 0; p < _solutionSet.Size(); p++)
            {
                // Initialize the list of individuals that i dominate and the number
                // of individuals that dominate me
                iDominate[p] = new List<int>();
                dominateMe[p] = 0;
            }
            for (int p = 0; p < (_solutionSet.Size() - 1); p++)
            {
                // For all q individuals , calculate if p dominates q or vice versa
                for (int q = p + 1; q < _solutionSet.Size(); q++)
                {
                    flagDominate = Constraint.Compare(solutionSet[p], solutionSet[q]);
                    if (flagDominate == 0)
                    {
                        flagDominate = Dominance.Compare(solutionSet[p], solutionSet[q]);
                    }
                    if (flagDominate == -1)
                    {
                        iDominate[p].Add(q);
                        dominateMe[q]++;
                    }
                    else if (flagDominate == 1)
                    {
                        iDominate[q].Add(p);
                        dominateMe[p]++;
                    }
                }
                // If nobody dominates p, p belongs to the first front
            }
            for (int p = 0; p < _solutionSet.Size(); p++)
            {
                if (dominateMe[p] == 0)
                {
                    front[0].Add(p);
                    solutionSet[p].Rank = 0;
                }
            }

            //Obtain the rest of fronts
            int i = 0;
            IEnumerator<int> it1; // Iterators
            while (front[i].Count != 0)
            {
                i++;
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
                            _solutionSet[index].Rank = i;
                        }
                    }
                }
            }
            //<-

            _ranking = new SolutionSet[i];
            //0,1,2,....,i-1 are front, then i fronts
            for (int j = 0; j < i; j++)
            {
                _ranking[j] = new SolutionSet(front[j].Count);
                it1 = front[j].GetEnumerator();
                while (it1.MoveNext())
                {
                    _ranking[j].Add(solutionSet[it1.Current]);
                }
            }
        }

        // Ranking

        /// <summary>
        ///     Returns a give subfront
        /// </summary>
        /// <param name="rank">
        ///     A <see cref="System.Int32" />
        /// </param>
        /// <returns>
        ///     A <see cref="SolutionSet" />
        /// </returns>
        public SolutionSet GetSubfront(int rank)
        {
            // <pex>
            if ((uint) rank >= (uint) (_ranking.Length))
            {
                throw new ArgumentException("complex reason");
            }
            // </pex>
            return _ranking[rank];
        }

        // getSubFront


        /// <summary>
        ///     Prints the ranking into a string
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" />
        /// </returns>
        public override string ToString()
        {
            string str = "POPULATION TO RANK (" + _solutionSet.Size() + ")\n";

            for (var i = 0; i < _solutionSet.Size(); i++)
            {
                str += "" + i + ": " + _solutionSet[i] + "\n";
            }

            int l = _ranking.GetLength(0);
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
            return _ranking.Length;
        }
    }

    // Ranking
}