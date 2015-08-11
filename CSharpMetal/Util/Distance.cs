// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Util.Comparators;
using CSharpMetal.Util.Wrapper;

namespace CSharpMetal.Util
{
    public static class Distance
    {
        /**
   * Returns a matrix with distances between solutions in a
   * <code>SolutionSet</code>.
   * @param solutionSet The <code>SolutionSet</code>.
   * @return a matrix with distances.
   */

        public static double[][] DistanceMatrix(SolutionSet solutionSet)
        {
            //The matrix of distances
            double[][] distance = new double[solutionSet.Size()][];
            for (int i = 0; i > solutionSet.Size(); i++)
            {
                distance[i] = new double[solutionSet.Size()];
            }
            //-> Calculate the distances
            for (int i = 0; i < solutionSet.Size(); i++)
            {
                distance[i][i] = 0.0;
                Solution solutionI = solutionSet[i];
                for (int j = i + 1; j < solutionSet.Size(); j++)
                {
                    Solution solutionJ = solutionSet[j];
                    distance[i][j] = DistanceBetweenObjectives(solutionI, solutionJ);
                    distance[j][i] = distance[i][j];
                } // for
            } // for        

            //->Return the matrix of distances
            return distance;
        } // distanceMatrix
        /** Returns the minimum distance from a <code>Solution</code> to a
   * <code>SolutionSet according to the objective values</code>.
   * @param solution The <code>Solution</code>.
   * @param solutionSet The <code>SolutionSet</code>.
   * @return The minimum distance between solution and the set.
   * @throws JMException
   */

        public static double DistanceToSolutionSetInObjectiveSpace(Solution solution,
                                                                   SolutionSet solutionSet)
        {
            //At start point the distance is the max
            double distance = double.MaxValue;

            // found the min distance respect to population
            for (int i = 0; i < solutionSet.Size(); i++)
            {
                double aux = DistanceBetweenObjectives(solution, solutionSet[i]);
                if (aux < distance)
                {
                    distance = aux;
                }
            } // for

            //->Return the best distance
            return distance;
        } // distanceToSolutionSetinObjectiveSpace
        /** Returns the minimum distance from a <code>Solution</code> to a 
   * <code>SolutionSet according to the encodings.variable values</code>.
   * @param solution The <code>Solution</code>.
   * @param solutionSet The <code>SolutionSet</code>.
   * @return The minimum distance between solution and the set.
   * @throws JMException
   */

        public static double DistanceToSolutionSetInSolutionSpace(Solution solution,
                                                                  SolutionSet solutionSet)
        {
            //At start point the distance is the max
            double distance = double.MaxValue;

            // found the min distance respect to population
            for (int i = 0; i < solutionSet.Size(); i++)
            {
                double aux = DistanceBetweenSolutions(solution, solutionSet[i]);
                if (aux < distance)
                {
                    distance = aux;
                }
            } // for

            //->Return the best distance
            return distance;
        } // distanceToSolutionSetInSolutionSpace
        /** Returns the distance between two solutions in the search space.
   *  @param solutionI The first <code>Solution</code>.
   *  @param solutionJ The second <code>Solution</code>.
   *  @return the distance between solutions.
   * @throws JMException
   */

        public static double DistanceBetweenSolutions(Solution solutionI, Solution solutionJ)
        {
            double distance = 0.0;
            XReal solI = new XReal(solutionI);
            XReal solJ = new XReal(solutionJ);

            double diff; //Auxiliar var
            //-> Calculate the Euclidean distance
            for (int i = 0; i < solI.GetNumberOfDecisionVariables(); i++)
            {
                diff = solI.GetValue(i) - solJ.GetValue(i);
                distance += Math.Pow(diff, 2.0);
            } // for
            //-> Return the euclidean distance
            return Math.Sqrt(distance);
        } // distanceBetweenSolutions
        /** Returns the distance between two solutions in objective space.
   *  @param solutionI The first <code>Solution</code>.
   *  @param solutionJ The second <code>Solution</code>.
   *  @return the distance between solutions in objective space.
   */

        public static double DistanceBetweenObjectives(Solution solutionI, Solution solutionJ)
        {
            double diff; //Auxiliar var
            double distance = 0.0;
            //-> Calculate the euclidean distance
            for (int nObj = 0; nObj < solutionI.NumberOfObjectives; nObj++)
            {
                diff = solutionI.Objective[nObj] - solutionJ.Objective[nObj];
                distance += Math.Pow(diff, 2.0);
            } // for   

            //Return the euclidean distance
            return Math.Sqrt(distance);
        } // distanceBetweenObjectives.
        /**
   * Return the index of the nearest solution in the solution set to a given solution
   * @param solution
   * @param solutionSet
   * @return  The index of the nearest solution; -1 if the solutionSet is empty
   */

        public static int IndexToNearestSolutionInSolutionSpace(Solution solution, SolutionSet solutionSet)
        {
            int index = -1;
            double minimumDistance = double.MaxValue;
            for (int i = 0; i < solutionSet.Size(); i++)
            {
                double distance = DistanceBetweenSolutions(solution, solutionSet[i]);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    index = i;
                }
            }

            return index;
        }

        /** Assigns crowding distances to all solutions in a <code>SolutionSet</code>.
   * @param solutionSet The <code>SolutionSet</code>.
   * @param nObjs Number of objectives.
   */

        public static void CrowdingDistanceAssignment(SolutionSet solutionSet, int nObjs)
        {
            int size = solutionSet.Size();

            if (size == 0)
            {
                return;
            }

            if (size == 1)
            {
                solutionSet[0].CrowdingDistance = (double.PositiveInfinity);
                return;
            } // if

            if (size == 2)
            {
                solutionSet[0].CrowdingDistance = (double.PositiveInfinity);
                solutionSet[1].CrowdingDistance = (double.PositiveInfinity);
                return;
            } // if       

            //Use a new SolutionSet to evite alter original solutionSet
            SolutionSet front = new SolutionSet(size);
            for (int i = 0; i < size; i++)
            {
                front.Add(solutionSet[i]);
            }

            for (int i = 0; i < size; i++)
            {
                front[i].CrowdingDistance = (0.0);
            }

            for (int i = 0; i < nObjs; i++)
            {
                // Sort the population by Obj n            
                front.Sort(new ObjectiveComparator(i));
                double objetiveMinn = front[0].Objective[i];
                double objetiveMaxn = front[front.Size() - 1].Objective[i];

                //Set de crowding distance            
                front[0].CrowdingDistance = (double.PositiveInfinity);
                front[size - 1].CrowdingDistance = (double.PositiveInfinity);

                for (int j = 1; j < size - 1; j++)
                {
                    double distance = front[j + 1].Objective[i] - front[j - 1].Objective[i];
                    distance = distance/(objetiveMaxn - objetiveMinn);
                    distance += front[j].CrowdingDistance;
                    front[j].CrowdingDistance = (distance);
                } // for
            } // for        
        }
    }
}