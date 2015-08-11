// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CSharpMetal.Core
{
    public class SolutionSet : IEnumerable
    {
        protected List<Solution> SolutionsList;
        public int Capacity { get; set; }

        public List<Solution> SolutionList
        {
            get { return SolutionsList; }
            set { SolutionList = value; }
        }

        /// <summary>
        ///     Indexer
        /// </summary>
        /// <param name="index">
        ///     A <see cref="System.Int32" />
        /// </param>
        public Solution this[int index]
        {
            get
            {
                if ((index >= SolutionList.Count) || (index < 0))
                {
                    throw new ArgumentNullException("Solution this[int index]");
                }
                return SolutionsList[index];
            }
            set
            {
                if ((index >= SolutionList.Count) || (index < 0))
                {
                    throw new ArgumentNullException("Solution this[int index]");
                }
                SolutionsList[index] = value;
            }
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public SolutionSet()
        {
            SolutionsList = new List<Solution>();
        }

        // SolutionSet
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="maximumSize">
        ///     A <see cref="System.Int32" />
        /// </param>
        public SolutionSet(int maximumSize)
        {
            SolutionsList = new List<Solution>();
            Capacity = maximumSize;
        }

        /// <summary>
        ///     Iterator
        /// </summary>
        /// <returns>
        ///     A <see cref="IEnumerator" />
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < SolutionsList.Count; i++)
            {
                yield return SolutionsList[i];
            }
        }

        // SolutionSet
        /// <summary>
        ///     Adds a new solution into the solution set
        /// </summary>
        /// <param name="solution">
        ///     A <see cref="Solution" />
        /// </param>
        /// <returns>
        ///     A <see cref="System.Boolean" />
        /// </returns>
        public virtual bool Add(Solution solution)
        {
            if (SolutionsList.Count == Capacity)
            {
                return false;
                // if
            }
            SolutionsList.Add(solution);
            return true;
            // else
        }

        // add
        /// <summary>
        ///     Returns the number of elements in the solution set
        /// </summary>
        /// <returns>
        ///     Number of elements in the solution set
        ///     A <see cref="System.Int32" />
        /// </returns>
        public int Size()
        {
            return SolutionsList.Count;
        }

        // size
        /// <summary>
        ///     Empties the solution set
        /// </summary>
        public void Clear()
        {
            SolutionsList.Clear();
        }

        /// <summary>
        ///     Joins two solution sets: the current one and the one passed as argument
        /// </summary>
        /// <param name="solutionSet">
        ///     A <see cref="SolutionSet" />
        /// </param>
        /// <returns>
        ///     A new solution set
        ///     A <see cref="SolutionSet" />
        /// </returns>
        public SolutionSet Union(SolutionSet solutionSet)
        {
            if (solutionSet == null)
            {
                throw new ArgumentNullException("solutionSet");
            }
            //Check the correct size
            int newSize = Size() + solutionSet.Size();
            if (newSize < Capacity)
            {
                newSize = Capacity;
            }

            //Create a new population
            var union = new SolutionSet(newSize);
            foreach (Solution solution in SolutionList)
            {
                union.Add(solution);
            }

            for (int i = Size(); i < (Size() + solutionSet.Size()); i++)
            {
                union.Add(solutionSet.SolutionList[i - Size()]);
            }

            return union;
        }

        // union

        public void Replace(int position, Solution solution)
        {
            if (position > SolutionsList.Count)
            {
                SolutionsList.Add(solution);
            } // if
            SolutionsList.RemoveAt(position);
            SolutionsList.Insert(position, solution);
        } // replace

        public void PrintObjectivesToFile(String path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.Length == 0)
            {
                throw new ArgumentException("path.Length == 0", "path");
            }

            var culture = new CultureInfo("EN-us");
            TextWriter tw = new StreamWriter(path);
            foreach (Solution solution in SolutionList)
            {
                foreach (double objective in solution.Objective)
                {
                    tw.Write(objective.ToString(culture.NumberFormat) + " ");
                }
                tw.WriteLine();
            }

            tw.Close();
        }

        // printObjectivesToFile
        public void PrintVariablesToFile(String path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.Length == 0)
            {
                throw new ArgumentException("path.Length == 0", "path");
            }


            TextWriter tw = new StreamWriter(path);
            foreach (Solution solution in SolutionList)
            {
                foreach (BaseVariable variable in solution.DecisionVariables)
                {
                    tw.Write(variable + " ");
                }

                tw.WriteLine();
            }

            tw.Close();
        }

        public void Sort(IComparer comparer)
        {
            if (comparer == null)
            {
                throw new Exception("No criterium for comparing exist");
            }
            SolutionList.Sort(comparer.Compare);
        }

        public double[][] WriteObjectivesToMatrix()
        {
            if (Size() == 0)
            {
                return null;
            }
            var objectives = new double[Size()][];
            for (int index = 0; index < Size(); index++)
            {
                objectives[index] = new double[this[0].NumberOfObjectives];
            }
            for (int i = 0; i < Size(); i++)
            {
                for (int j = 0; j < this[0].NumberOfObjectives; j++)
                {
                    objectives[i][j] = this[i].Objective[j];
                }
            }
            return objectives;
        }

        public void Remove(int i)
        {
            if (i > SolutionList.Count - 1)
            {
                throw new IndexOutOfRangeException("There is no solution to delete at index: " + i);
            }
            SolutionList.RemoveAt(i);
        }

        public int IndexWorst(IComparer comparator)
        {
            if ((SolutionList == null) || (SolutionList.Count == 0))
            {
                return -1;
            }
            int index = 0;
            Solution worstKnown = SolutionList[0];
            for (int i = 1; i < SolutionList.Count; i++)
            {
                Solution candidateSolution = SolutionList[i];
                int flag = comparator.Compare(worstKnown, candidateSolution);
                if (flag == -1)
                {
                    index = i;
                    worstKnown = candidateSolution;
                }
            }

            return index;
        }

        public void PrintFeasibleFUN(string path)
        {
            using (TextWriter myStreamWriter = new StreamWriter(path))
            {
                SolutionsList.ToList().ForEach(aSolution =>
                                               {
                                                   if (Math.Abs(aSolution.OverallConstraintViolation) < Double.Epsilon)
                                                   {
                                                       myStreamWriter.WriteLine(aSolution);
                                                   }
                                               });
            }
        }
    }
}