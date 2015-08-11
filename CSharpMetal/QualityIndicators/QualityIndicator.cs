// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.QualityIndicators.Util;

namespace CSharpMetal.QualityIndicators
{
    public class QualityIndicator
    {
        private readonly Problem _problem;
        private readonly SolutionSet _trueParetoFront;
        private readonly double _trueParetoFrontHypervolume;

        public QualityIndicator(Problem problem, String paretoFrontFile)
        {
            _problem = problem;
            _trueParetoFront = MetricsUtil.ReadNonDominatedSolutionSet(paretoFrontFile);
            _trueParetoFrontHypervolume = new Hypervolume().HypervolumeValue(
                _trueParetoFront.WriteObjectivesToMatrix(),
                _trueParetoFront.WriteObjectivesToMatrix(),
                _problem.NumberOfObjectives);
        } // Constructor 
        /**
         * Returns the hypervolume of solution set
         * @param solutionSet Solution set
         * @return The value of the hypervolume indicator
         */

        public double GetHypervolume(SolutionSet solutionSet)
        {
            return new Hypervolume().HypervolumeValue(solutionSet.WriteObjectivesToMatrix(),
                                                      _trueParetoFront.WriteObjectivesToMatrix(),
                                                      _problem.NumberOfObjectives);
        } // getHypervolume
        /**
         * Returns the hypervolume of the true Pareto front
         * @return The hypervolume of the true Pareto front
         */

        public double GetTrueParetoFrontHypervolume()
        {
            return _trueParetoFrontHypervolume;
        }

        /**
         * Returns the inverted generational distance of solution set
         * @param solutionSet Solution set
         * @return The value of the hypervolume indicator
         */

        public double GetIgd(SolutionSet solutionSet)
        {
            return new InvertedGenerationalDistance().Compute(
                solutionSet.WriteObjectivesToMatrix(),
                _trueParetoFront.WriteObjectivesToMatrix(),
                _problem.NumberOfObjectives);
        } // getIGD
        /**
          * Returns the generational distance of solution set
         * @param solutionSet Solution set
          * @return The value of the hypervolume indicator
          */

        public double GetGd(SolutionSet solutionSet)
        {
            return new GenerationalDistance().Compute(
                solutionSet.WriteObjectivesToMatrix(),
                _trueParetoFront.WriteObjectivesToMatrix(),
                _problem.NumberOfObjectives);
        } // getGD
        /**
         * Returns the spread of solution set
         * @param solutionSet Solution set
         * @return The value of the hypervolume indicator
         */

        public double GetSpread(SolutionSet solutionSet)
        {
            return new Spread().Compute(solutionSet.WriteObjectivesToMatrix(),
                                        _trueParetoFront.WriteObjectivesToMatrix(),
                                        _problem.NumberOfObjectives);
        } // getGD
        /**
       * Returns the epsilon indicator of solution set
         * @param solutionSet Solution set
       * @return The value of the hypervolume indicator
       */

        public double GetEpsilon(SolutionSet solutionSet)
        {
            return new Epsilon().Compute(solutionSet.WriteObjectivesToMatrix(),
                                         _trueParetoFront.WriteObjectivesToMatrix(),
                                         _problem.NumberOfObjectives);
        }
    }
}