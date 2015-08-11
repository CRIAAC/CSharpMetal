// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System.Collections;
using CSharpMetal.Core;
using CSharpMetal.Util.Comparators;

namespace CSharpMetal.Util
{
    public class NonDominatedSolutionList : SolutionSet
    {
        /**
	 * Stores a <code>Comparator</code> for checking if two solutions are equal
	 */
        private static IComparer _equal = new SolutionComparator();
        /**
	 * Stores a <code>Comparator</code> for dominance checking
	 */
        private readonly IComparer _dominance = new DominanceComparator();
        /** 
	 * Constructor.
	 * The objects of this class are lists of non-dominated solutions according to
	 * a Pareto dominance comparator. 
	 */

        public NonDominatedSolutionList()
        {
        } // NonDominatedList
        /**
	 * Constructor.
	 * This constructor creates a list of non-dominated individuals using a
	 * comparator object.
	 * @param dominance The comparator for dominance checking.
	 */

        public NonDominatedSolutionList(IComparer dominance)
        {
            _dominance = dominance;
        } // NonDominatedList
        /** Inserts a solution in the list
	 * @param solution The solution to be inserted.
	 * @return true if the operation success, and false if the solution is 
	 * dominated or if an identical individual exists.
	 * The decision variables can be null if the solution is read from a file; in
	 * that case, the domination tests are omitted
	 */

        public override bool Add(Solution solution)
        {
            if (SolutionList.Count == 0)
            {
                SolutionList.Add(solution);
                return true;
            }
            for (int index = SolutionList.Count - 1; index >= 0; index--)
            {
                Solution listIndividual = SolutionList[index];
                int flag = _dominance.Compare(solution, listIndividual);

                if (flag == -1)
                {
                    // A solution in the list is dominated by the new one
                    SolutionList.RemoveAt(index);
                }
                else if (flag == 0)
                {
                    // Non-dominated solutions
                    //flag = equal_.compare(solution,listIndividual);
                    //if (flag == 0) {
                    //	return false;   // The new solution is in the list  
                    //}
                }
                else if (flag == 1)
                {
                    // The new solution is dominated
                    return false;
                }
            }

            //At this point, the solution is inserted into the list
            SolutionList.Add(solution);

            return true;
        }
    }
}