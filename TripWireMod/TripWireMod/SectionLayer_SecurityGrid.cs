using RimWorld;
using System.Collections.Generic;
using System.Linq;
using TripWireMod;
using UnityEngine;
using Verse;

namespace TripWireTrap
{
    public class SectionLayer_SecurityGrid : SectionLayer
    {
        public SectionLayer_SecurityGrid(Section section) : base(section)
        {
            this.relevantChangeTypes = MapMeshFlag.Things;
            this.relevantChangeTypes |= MapMeshFlag.Buildings;
        }

        public override void Regenerate()
        {
            this.ClearSubMeshes(Verse.MeshParts.All);

            if (!MapComponent_TripWire.SecurityGridActive)
            {
                return;
            }

            List<Thing> allTripWireBuildings = base.Map.listerThings.ThingsMatching(ThingRequest.ForDef(TripWireTrapDefOf.TripWireTrap)).ToList();

            foreach (Thing thing in allTripWireBuildings)
            {
                if (!this.section.CellRect.Contains(thing.Position))
                {
                    continue;
                }

                Building_TripWire tripWireBuilding = thing as Building_TripWire;
                if (tripWireBuilding == null) continue; 

                CompTripWire compTripWire = tripWireBuilding.GetComp<CompTripWire>();
                if (compTripWire != null)
                {
                    compTripWire.PrintSecurityOverlay(this);
                }
            }
        }
    }
}