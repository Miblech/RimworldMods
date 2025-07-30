using Verse;

namespace TrapsExpanded
{
    public class PlaceWorker_LandslideTrapOnlyOnNaturalRock : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            IntVec3 wallCell = TrapUtils.DetermineWallCell(loc, rot);

            if (!wallCell.InBounds(map))
            {
                return "Must be placed against a natural rock wall."; 
            }

            foreach (Thing foundThing in wallCell.GetThingList(map))
            {
                if (foundThing.def.building != null && foundThing.def.building.isNaturalRock)
                {
                    return true;
                }
            }

            return "Must be placed against a natural rock wall.";
        }
    }
}