// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Util.Archives
{
    internal class AdaptiveGridArchive : BaseArchive
    {
        /**
         * Stores a <code>Comparator</code> for dominance checking
         */
        private readonly IComparer _dominance;
        /** 
         * Stores the maximum size of the archive
         */
        private readonly int _maxSize;
        /** 
         * Stores the adaptive grid
        */
        public AdaptiveGrid Grid { get; private set; }

        public AdaptiveGridArchive(int maxSize, int bisections, int objectives) : base(maxSize)
        {
            _maxSize = maxSize;
            _dominance = new DominanceComparator();
            Grid = new AdaptiveGrid(bisections, objectives);
        }

        /**
	 * Adds a <code>Solution</code> to the archive. If the <code>Solution</code>
	 * is dominated by any member of the archive then it is discarded. If the 
	 * <code>Solution</code> dominates some members of the archive, these are
	 * removed. If the archive is full and the <code>Solution</code> has to be
	 * inserted, one <code>Solution</code> of the most populated hypercube of the
	 * adaptive grid is removed.
	 * @param solution The <code>Solution</code>
	 * @return true if the <code>Solution</code> has been inserted, false
	 * otherwise.
	 */

        public override bool Add(Solution solution)
        {
            for (int i = SolutionList.Count - 1; i >= 0; --i)
            {
                Solution element = SolutionList[i];
                int flag = _dominance.Compare(solution, element);
                if (flag == -1)
                {
                    // The Individual to insert dominates other 
                    // individuals in  the archive
                    SolutionList.RemoveAt(i); //Delete it from the archive
                    int localLocation = Grid.Location(element);
                    if (Grid.Hypercubes[localLocation] > 1)
                    {
//The hypercube contains 
                        Grid.RemoveSolution(localLocation); //more than one individual
                    }
                    else
                    {
                        Grid.updateGrid(this);
                    } // else
                } // if 
                else if (flag == 1)
                {
                    // An Individual into the file dominates the 
                    // solution to insert
                    return false; // The solution will not be inserted
                } // else if           
            } // while

            // At this point, the solution may be inserted
            if (Size() == 0)
            {
                //The archive is empty
                SolutionList.Add(solution);
                Grid.updateGrid(this);
                return true;
            } //

            if (Size() < _maxSize)
            {
                //The archive is not full              
                Grid.updateGrid(solution, this); // Update the grid if applicable
                int localLocation = Grid.Location(solution);
                Grid.AddSolution(localLocation); // Increment the density of the hypercube
                SolutionList.Add(solution); // Add the solution to the list
                return true;
            } // if

            // At this point, the solution has to be inserted and the archive is full
            Grid.updateGrid(solution, this);
            int location = Grid.Location(solution);
            if (location == Grid.MostPopulated)
            {
                // The solution is in the 
                // most populated hypercube
                return false; // Not inserted
            }
            // Remove an solution from most populated area
            for (int i = SolutionList.Count - 1; i >= 0; --i)

            {
                Solution element = SolutionList[i];
                int location2 = Grid.Location(element);
                if (location2 == Grid.MostPopulated)
                {
                    SolutionList.RemoveAt(i);
                    Grid.RemoveSolution(location2);
                } // if
            } // while
            // A solution from most populated hypercube has been removed, 
            // insert now the solution
            Grid.AddSolution(location);
            SolutionsList.Add(solution);
            return true;
        }
    }
}