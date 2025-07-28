using RimWorld;
using System.Collections.Generic;
using Verse;

namespace TrapsExpanded
{
    public class Building_TrapTriggerer : Building_Trap
    {

        static Building_TrapTriggerer()
        {
            Log.Message("[TrapsExpanded] Building_TrapTriggerer class loaded!");
        }

        protected override void Tick()
        {
            base.Tick();
        }
        protected override void SpringSub(Pawn p)
        {
            Log.Message("[TrapsExpanded] SpringSub triggered by: " + p.Name);
            this.OnTrapTriggered(p);
        }

        protected virtual void OnTrapTriggered(Pawn p)
        {
            Log.Message("[TrapsExpanded] OnTrapTriggered method triggered by: " + p.LabelShort);
            TriggerConnectedIEDs(this.Position, this.Map, p);
        }

        private void TriggerConnectedIEDs(IntVec3 origin, Map map, Pawn triggerer)
        {
            HashSet<IntVec3> visited = new HashSet<IntVec3>();
            Queue<IntVec3> toVisit = new Queue<IntVec3>();
            toVisit.Enqueue(origin);

            while (toVisit.Count > 0)
            {
                IntVec3 current = toVisit.Dequeue();
                if (!visited.Add(current)) continue;

                List<Thing> things = map.thingGrid.ThingsListAt(current);
                foreach (Thing t in things)
                {
                    if (IsIED(t.def))
                    {
                        Log.Message($"[TrapsExpanded] Triggering IED at {t.Position}");
                        var explosive = t.TryGetComp<CompExplosive>();

                        if (explosive != null && !explosive.wickStarted)
                        {
                            explosive.StartWick(null);
                        }
                    }


                    if (HasTripWireOrIEDAt(current, map))
                    {
                        foreach (var dir in GenAdj.CardinalDirections)
                        {
                            IntVec3 neighbor = current + dir;
                            if (neighbor.InBounds(map) && !visited.Contains(neighbor) && HasTripWireOrIEDAt(neighbor, map))
                            {
                                toVisit.Enqueue(neighbor);
                            }
                        }
                    }
                }
            }
        }

        private bool HasTripWireOrIEDAt(IntVec3 pos, Map map)
        {
            var things = map.thingGrid.ThingsListAt(pos);
            foreach (Thing t in things)
            {
                if (t.def.defName == "TrapsExpanded" || IsIED(t.def))
                    return true;
            }
            return false;
        }

        private bool IsIED(ThingDef def)
        {
            return def.defName.StartsWith("TrapIED_");
        }
    }
}