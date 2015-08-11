// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System;
using System.Linq;
using CSharpMetal.Core;

namespace CSharpMetal.Util
{
    public class AdaptiveGrid
    {
        /**
         * Size of hypercube for each dimension
         */
        private readonly double[] _divisionSize;
        /**
         * 
         * Grid lower bounds
         */
        private readonly double[] _lowerLimits;
        /**
         * Objectives of the problem
         */
        private readonly int _objectives;
        /**
         * Grid upper bounds
         */
        private readonly double[] _upperLimits;
        /**
         * Hndicates when an hypercube has solutions
         */
        private int[] _occupied;
        /**
       * Number of bi-divisions of the objective space
       */
        public int Bisections { get; private set; }
        /**
         * Number of solutions into a specific hypercube in the adaptative grid
         */
        public int[] Hypercubes { get; private set; }
        /**
         * Hypercube with maximum number of solutions
         */
        public int MostPopulated { get; private set; }

        public AdaptiveGrid(int bisections, int objectives)
        {
            Bisections = bisections;
            _objectives = objectives;
            _lowerLimits = new double[_objectives];
            _upperLimits = new double[_objectives];
            _divisionSize = new double[_objectives];
            Hypercubes = new int[(int) Math.Pow(2.0, Bisections*_objectives)];
        } //AdaptativeGrid
        /**
        * Constructor.
        * Creates an instance of AdaptativeGrid.
        * @param bisections Number of bi-divisions of the objective space.
        * @param objetives Number of objectives of the problem.
        */

        public int GetLocationDensity(int location)
        {
            return Hypercubes[location];
        }

        /** 
        *  Updates the grid limits considering the solutions contained in a 
        *  <code>SolutionSet</code>.
        *  @param solutionSet The <code>SolutionSet</code> considered.
        */

        private void UpdateLimits(SolutionSet solutionSet)
        {
            //Init the lower and upper limits 
            for (int obj = 0; obj < _objectives; obj++)
            {
                //Set the lower limits to the max real
                _lowerLimits[obj] = double.MaxValue;
                //Set the upper limits to the min real
                _upperLimits[obj] = double.MinValue;
            }

            //Find the max and min limits of objetives into the population
            for (int ind = 0; ind < solutionSet.Size(); ind++)
            {
                Solution tmpIndividual = solutionSet[ind];
                for (int obj = 0; obj < _objectives; obj++)
                {
                    double tmpIndividualObjective = tmpIndividual.Objective[obj];
                    if (tmpIndividualObjective < _lowerLimits[obj])
                    {
                        _lowerLimits[obj] = tmpIndividualObjective;
                    }
                    if (tmpIndividualObjective > _upperLimits[obj])
                    {
                        _upperLimits[obj] = tmpIndividualObjective;
                    }
                }
            }
        } //updateLimits
        /** 
         * Updates the grid adding solutions contained in a specific 
         * <code>SolutionSet</code>.
         * <b>REQUIRE</b> The grid limits must have been previously calculated.
         * @param solutionSet The <code>SolutionSet</code> considered.
         */

        private void AddSolutionSet(SolutionSet solutionSet)
        {
            //Calculate the location of all individuals and update the grid
            MostPopulated = 0;

            for (int ind = 0; ind < solutionSet.Size(); ind++)
            {
                int location = Location(solutionSet[ind]);
                Hypercubes[location]++;
                if (Hypercubes[location] > Hypercubes[MostPopulated])
                {
                    MostPopulated = location;
                }
            }

            //The grid has been updated, so also update ocuppied's hypercubes
            CalculateOccupied();
        } // addSolutionSet
        /** 
         * Updates the grid limits and the grid content adding the solutions contained
         * in a specific <code>SolutionSet</code>.
         * @param solutionSet The <code>SolutionSet</code>.
         */

        public void updateGrid(SolutionSet solutionSet)
        {
            //Update lower and upper limits
            UpdateLimits(solutionSet);

            //Calculate the division Size
            for (int obj = 0; obj < _objectives; obj++)
            {
                _divisionSize[obj] = _upperLimits[obj] - _lowerLimits[obj];
            }

            //Clean the hypercubes
            for (int i = 0; i < Hypercubes.Length; i++)
            {
                Hypercubes[i] = 0;
            }

            //Add the population
            AddSolutionSet(solutionSet);
        } //updateGrid
        /** 
         * Updates the grid limits and the grid content adding a new 
         * <code>Solution</code>.
         * If the solution falls out of the grid bounds, the limits and content of the
         * grid must be re-calculated.
         * @param solution <code>Solution</code> considered to update the grid.
         * @param solutionSet <code>SolutionSet</code> used to update the grid.
         */

        public void updateGrid(Solution solution, SolutionSet solutionSet)
        {
            int location = Location(solution);
            if (location == -1)
            {
//Re-build the Adaptative-Grid
                //Update lower and upper limits
                UpdateLimits(solutionSet);

                //Actualize the lower and upper limits whit the individual      
                for (int obj = 0; obj < _objectives; obj++)
                {
                    double solutionObjective = solution.Objective[obj];

                    if (solutionObjective < _lowerLimits[obj])
                    {
                        _lowerLimits[obj] = solutionObjective;
                    }
                    if (solutionObjective > _upperLimits[obj])
                    {
                        _upperLimits[obj] = solutionObjective;
                    }
                }

                //Calculate the division Size
                for (int obj = 0; obj < _objectives; obj++)
                {
                    _divisionSize[obj] = _upperLimits[obj] - _lowerLimits[obj];
                }

                //Clean the hypercube
                for (int i = 0; i < Hypercubes.Length; i++)
                {
                    Hypercubes[i] = 0;
                }

                //add the population
                AddSolutionSet(solutionSet);
            }
        } //updateGrid
        /** 
         * Calculates the hypercube of a solution.
         * @param solution The <code>Solution</code>.
         */

        public int Location(Solution solution)
        {
            //Create a int [] to store the range of each objetive
            int[] position = new int[_objectives];

            //Calculate the position for each objetive
            for (int obj = 0; obj < _objectives; obj++)
            {
                if ((solution.Objective[obj] > _upperLimits[obj])
                    || (solution.Objective[obj] < _lowerLimits[obj]))
                {
                    return -1;
                }
                if (Math.Abs(solution.Objective[obj] - _lowerLimits[obj]) < double.Epsilon)
                {
                    position[obj] = 0;
                }
                else if (Math.Abs(solution.Objective[obj] - _upperLimits[obj]) < double.Epsilon)
                {
                    position[obj] = ((int) Math.Pow(2.0, Bisections)) - 1;
                }
                else
                {
                    double tmpSize = _divisionSize[obj];
                    double value = solution.Objective[obj];
                    double account = _lowerLimits[obj];
                    int ranges = (int) Math.Pow(2.0, Bisections);
                    for (int b = 0; b < Bisections; b++)
                    {
                        tmpSize /= 2.0;
                        ranges /= 2;
                        if (value > (account + tmpSize))
                        {
                            position[obj] += ranges;
                            account += tmpSize;
                        }
                    }
                }
            }

            //Calcualate the location into the hypercubes
            int location = 0;
            for (int obj = 0; obj < _objectives; obj++)
            {
                location += (int) (position[obj]*Math.Pow(2.0, obj*Bisections));
            }
            return location;
        } //location
        /** 
        * Decreases the number of solutions into a specific hypercube.
        * @param location Number of hypercube.
        */

        public void RemoveSolution(int location)
        {
            //Decrease the solutions in the location specified.
            Hypercubes[location]--;

            //Update the most poblated hypercube
            if (location == MostPopulated)
            {
                for (int i = 0; i < Hypercubes.Length; i++)
                {
                    if (Hypercubes[i] > Hypercubes[MostPopulated])
                    {
                        MostPopulated = i;
                    }
                }
            }

            //If hypercubes[location] now becomes to zero, then update ocupped hypercubes
            if (Hypercubes[location] == 0)
            {
                CalculateOccupied();
            }
        } //removeSolution
        /**
         * Increases the number of solutions into a specific hypercube.
         * @param location Number of hypercube.
         */

        public void AddSolution(int location)
        {
            //Increase the solutions in the location specified.
            Hypercubes[location]++;

            //Update the most poblated hypercube
            if (Hypercubes[location] > Hypercubes[MostPopulated])
            {
                MostPopulated = location;
            }

            //if hypercubes[location] becomes to one, then recalculate 
            //the occupied hypercubes
            if (Hypercubes[location] == 1)
            {
                CalculateOccupied();
            }
        } //addSolution
        /** 
         * Retunrns a String representing the grid.
         * @return The String.
         */

        public String toString()
        {
            String result = "Grid\n";
            for (int obj = 0; obj < _objectives; obj++)
            {
                result += "Objective " + obj + " " + _lowerLimits[obj] + " "
                          + _upperLimits[obj] + "\n";
            }
            return result;
        } // toString
        /** 
         * Returns a random hypercube using a rouleteWheel method.  
        *  @return the number of the selected hypercube.
        */

        public int RouletteWheel()
        {
            //Calculate the inverse sum
            double inverseSum = 0.0;
            foreach (int aHypercubes in Hypercubes)
            {
                if (aHypercubes > 0)
                {
                    inverseSum += 1.0/aHypercubes;
                }
            }

            //Calculate a random value between 0 and sumaInversa
            double random = PseudoRandom.Instance().NextDouble()*inverseSum;
            int hypercube = 0;
            double accumulatedSum = 0.0;
            while (hypercube < Hypercubes.Length)
            {
                if (Hypercubes[hypercube] > 0)
                {
                    accumulatedSum += 1.0/Hypercubes[hypercube];
                }

                if (accumulatedSum > random)
                {
                    return hypercube;
                }

                hypercube++;
            } // while

            return hypercube;
        } //rouletteWheel
        /**
        * Calculates the number of hypercubes having one or more solutions.
        * return the number of hypercubes with more than zero solutions.
        */

        public int CalculateOccupied()
        {
            int total = Hypercubes.Count(aHypercubes => aHypercubes > 0);

            _occupied = new int[total];
            int base_ = 0;
            for (int i = 0; i < Hypercubes.Length; i++)
            {
                if (Hypercubes[i] > 0)
                {
                    _occupied[base_] = i;
                    base_++;
                }
            }

            return total;
        } //calculateOcuppied
        /** 
         * Returns the number of hypercubes with more than zero solutions.
         * @return the number of hypercubes with more than zero solutions.
         */

        public int OccupiedHypercubes()
        {
            return _occupied.Length;
        } // occupiedHypercubes
        /**
         * Returns a random hypercube that has more than zero solutions.
         * @return The hypercube.
         */

        public int RandomOccupiedHypercube()
        {
            int rand = PseudoRandom.Instance().Next(0, _occupied.Length - 1);
            return _occupied[rand];
        }
    }
}