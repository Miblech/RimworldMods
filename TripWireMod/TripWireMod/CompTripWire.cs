using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TripWireTrap
{
    public class CompProperties_TripWire : CompProperties
    {
        public string tripWireNetDefName;

        public CompProperties_TripWire()
        {
            this.compClass = typeof(CompTripWire);
        }
    }

    public class CompTripWire : ThingComp
    {
        private TripWireNetDef cachedTripWireNetDef;
        public TripWireNetDef TripWireNetDef
        {
            get
            {
                if (cachedTripWireNetDef == null)
                {
                    CompProperties_TripWire props = (CompProperties_TripWire)this.props;
                    if (props != null && !props.tripWireNetDefName.NullOrEmpty())
                    {
                        cachedTripWireNetDef = DefDatabase<TripWireNetDef>.GetNamed(props.tripWireNetDefName, false);
                    }
                }
                return cachedTripWireNetDef;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {

            base.PostSpawnSetup(respawningAfterLoad);
            Log.Message($"CompTripWire: PostSpawnSetup called for {parent.LabelShort}.");
            parent.Map.mapDrawer.MapMeshDirty(parent.Position, (ulong)TripWireMeshFlag.SecurityGrid);
            Log.Message($"CompTripWire: Called MapMeshDirty from PostSpawnSetup for {parent.LabelShort}.");
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);

            Log.Message($"CompTripWire: PostDeSpawn called for {parent.LabelShort}.");
            if (map != null)
            {
                map.mapDrawer.MapMeshDirty(parent.Position, (ulong)TripWireMeshFlag.SecurityGrid);
                Log.Message($"CompTripWire: Called MapMeshDirty from PostDeSpawn for {parent.LabelShort}.");
            }
        }

        // Method to print the security overlay onto a given SectionLayer
        public void PrintSecurityOverlay(SectionLayer layer)
        {
            if (TripWireNetDef == null || TripWireNetDef.OverlayMaterial == null)
            {
                if (Current.ProgramState == ProgramState.Playing)
                {
                    Log.ErrorOnce($"CompTripWire: Material/Def missing for {parent.LabelShort}. TripWireNetDef: {TripWireNetDef?.defName ?? "null"}, OverlayMaterial: {(TripWireNetDef?.OverlayMaterial == null ? "null" : "found")}.", (int)(parent.thingIDNumber ^ 0xDEADBEEF));
                }
                return;
            }

            Material overlayMat = TripWireNetDef.OverlayMaterial;
            // Use MapDataOverlay for highlights, ensuring it draws above most things.
            float y = AltitudeLayer.MapDataOverlay.AltitudeFor();

            // 1. Draw the node graphic for this building (the center point).
            Printer_Plane.PrintPlane(layer, parent.TrueCenter(), new Vector2(1f, 1f), overlayMat);

            // 2. Draw lines to connected children (if this is a transmitter)
            CompTripWireTransmitter transmitterComp = parent.TryGetComp<CompTripWireTransmitter>();
            if (transmitterComp != null)
            {
                foreach (var childConnector in transmitterComp.connectChildren)
                {
                    if (childConnector?.parent != null)
                    {
                        Vector3 start = parent.TrueCenter();
                        Vector3 end = childConnector.parent.TrueCenter();
                        start.y = y; // Ensure lines are at the same altitude as the nodes
                        end.y = y;

                        // Calculate center and rotation for the line segment
                        Vector3 center = (start + end) / 2f;
                        Vector3 v = end - start;
                        Vector2 size = new Vector2(1f, v.MagnitudeHorizontal());
                        float rot = v.AngleFlat();

                        Printer_Plane.PrintPlane(layer, center, size, overlayMat, rot);
                    }
                }
            }

            // 3. Draw line to parent (if this is a connector and has a parent)
            CompTripWireConnector connectorComp = parent.TryGetComp<CompTripWireConnector>();
            if (connectorComp != null && connectorComp.connectParent != null)
            {
                Vector3 start = parent.TrueCenter();
                Vector3 end = connectorComp.connectParent.parent.TrueCenter();
                start.y = y; // Ensure lines are at the same altitude as the nodes
                end.y = y;

                // Calculate center and rotation for the line segment
                Vector3 center = (start + end) / 2f;
                Vector3 v = end - start;
                Vector2 size = new Vector2(1f, v.MagnitudeHorizontal());
                float rot = v.AngleFlat();

                Printer_Plane.PrintPlane(layer, center, size, overlayMat, rot);
            }
        }
    }
}