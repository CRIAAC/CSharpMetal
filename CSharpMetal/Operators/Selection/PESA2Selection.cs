// Author : Vandewynckel Julien
// Creation date : 07/03/2015
// Last modified date : 05/05/2015

using System.Collections.Generic;
using CSharpMetal.Core;
using CSharpMetal.Util;
using CSharpMetal.Util.Archives;

namespace CSharpMetal.Operators.Selection
{
    internal class PESA2Selection : BaseSelection
    {
        public PESA2Selection(Dictionary<string, object> parameters) : base(parameters)
        {
        }

        public override object Execute(object obj)
        {
            AdaptiveGridArchive archive = (AdaptiveGridArchive) obj;
            int selected;
            int hypercube1 = archive.Grid.RandomOccupiedHypercube();
            int hypercube2 = archive.Grid.RandomOccupiedHypercube();

            if (hypercube1 != hypercube2)
            {
                if (archive.Grid.Hypercubes[hypercube1] <
                    archive.Grid.Hypercubes[hypercube2])
                {
                    selected = hypercube1;
                }
                else if (archive.Grid.Hypercubes[hypercube2] <
                         archive.Grid.Hypercubes[hypercube1])
                {
                    selected = hypercube2;
                }
                else
                {
                    selected = PseudoRandom.Instance().NextDouble() < 0.5 ? hypercube2 : hypercube1;
                }
            }
            else
            {
                selected = hypercube1;
            }
            int base_ = PseudoRandom.Instance().Next(0, archive.Size() - 1);
            int cnt = 0;
            while (cnt < archive.Size())
            {
                Solution individual = archive[(base_ + cnt)%archive.Size()];
                if (archive.Grid.Location(individual) != selected)
                {
                    cnt++;
                }
                else
                {
                    return individual;
                }
            }
            return archive[(base_ + cnt)%archive.Size()];
        }
    }
}