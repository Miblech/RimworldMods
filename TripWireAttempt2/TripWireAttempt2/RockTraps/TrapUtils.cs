using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TrapsExpanded
{

    public static class TrapUtils
    {
        public static IntVec3 DetermineWallCell(IntVec3 originCell, Rot4 rotation)
    {
        if (rotation == Rot4.North) // If object faces North, the wall is to its South
        {
            return originCell + IntVec3.South;
        }
        else if (rotation == Rot4.East) // If object faces East, the wall is to its West
        {
            return originCell + IntVec3.West;
        }
        else if (rotation == Rot4.South) // If object faces South, the wall is to its North
        {
            return originCell + IntVec3.North;
        }
        else // rotation == Rot4.West (If object faces West, the wall is to its East)
        {
            return originCell + IntVec3.East;
        }
    }
}
}