// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Util.Archives
{
    public class CrowdingArchive : BaseArchive
    {
        /**
         * Stores a <code>Comparator</code> for checking crowding distances.
         */
        private readonly IComparer _crowdingDistance;
        /**
         * Stores a <code>Comparator</code> for dominance checking.
         */
        private readonly IComparer _dominance;
        /**
         * Stores a <code>Comparator</code> for equality checking (in the objective
         * space).
         */
        private readonly IComparer _equals;
        private readonly int _maxSize;
        /**
         * stores the number of the objectives.
         */
        private readonly int _objectives;
        /**
         * Constructor. 
         * @param maxSize The maximum size of the archive.
         * @param numberOfObjectives The number of objectives.
         */

        public CrowdingArchive(int maxSize, int numberOfObjectives) : base(maxSize)
        {
            _maxSize = maxSize;
            _objectives = numberOfObjectives;
            _dominance = new DominanceComparator();
            _equals = new EqualSolutions();
            _crowdingDistance = new CrowdingDistanceComparator();
        } // CrowdingArchive
        /**
         * Adds a <code>Solution</code> to the archive. If the <code>Solution</code>
         * is dominated by any member of the archive, then it is discarded. If the 
         * <code>Solution</code> dominates some members of the archive, these are
         * removed. If the archive is full and the <code>Solution</code> has to be
         * inserted, the solutions are sorted by crowding distance and the one having
         * the minimum crowding distance value.
         * @param solution The <code>Solution</code>
         * @return true if the <code>Solution</code> has been inserted, false 
         * otherwise.
         */

        public override bool Add(Solution solution)
        {
            int i = 0;
            while (i < SolutionList.Count)
            {
                Solution aux = SolutionList[i]; //Store an solution temporally

                int flag = _dominance.Compare(solution, aux);
                if (flag == 1)
                {
                    // The solution to add is dominated
                    return false; // Discard the new solution
                }
                if (flag == -1)
                {
                    // A solution in the archive is dominated
                    SolutionList.RemoveAt(i); // Remove it from the population            
                }
                else
                {
                    if (_equals.Compare(aux, solution) == 0)
                    {
                        // There is an equal solution 
                        // in the population
                        return false; // Discard the new solution
                    } // if
                    i++;
                }
            }
            // Insert the solution into the archive
            SolutionList.Add(solution);
            if (Size() > _maxSize)
            {
                // The archive is full
                Distance.CrowdingDistanceAssignment(this, _objectives);
                Remove(IndexWorst(_crowdingDistance));
            }
            return true;
        }
    }
}