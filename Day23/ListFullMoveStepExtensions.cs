using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public static class ListFullMoveStepExtensions
    {
        public static List<FullMoveStep> DeepCopy(this List<FullMoveStep> source)
        {
            var listCopy = new List<FullMoveStep>();

            foreach(FullMoveStep fms in source)
            {
                FullMoveStep newStep = new FullMoveStep()
                {
                    direction = fms.direction,
                    newColumn = fms.newColumn,
                    newRow = fms.newRow,
                    previousColumn = fms.previousColumn,
                    previousRow = fms.previousRow,
                    player = fms.player
                };
                    
                listCopy.Add(newStep);
            }

            return listCopy;
        }
    }
}
