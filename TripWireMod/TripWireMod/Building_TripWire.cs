using Verse;
using RimWorld;
using UnityEngine;

namespace TripWireTrap
{
    public class Building_TripWire : Building
    {
        // Called when the Thing is first created (e.g., from blueprint or Dev Mode)
        public override void PostMake()
        {
            base.PostMake();
            Log.Message($"Building_TripWire: PostMake called for {LabelShort} (ID: {thingIDNumber}).");
        }

        // Called when the Thing is placed on the map (or loaded from save)
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            Log.Message($"Building_TripWire: SpawnSetup called for {LabelShort} (ID: {thingIDNumber}) on map {map.Parent.Label}. Respawning: {respawningAfterLoad}.");

            // Explicitly mark our custom security grid mesh dirty for this building's position.
            // This tells the game that the SecurityGrid layer needs to be redrawn in this cell.
            map.mapDrawer.MapMeshDirty(Position, (ulong)TripWireMeshFlag.SecurityGrid);
            Log.Message($"Building_TripWire: Called MapMeshDirty from SpawnSetup for {LabelShort} at {Position} with SecurityGrid flag.");
        }

        // Called when the Thing is removed from the map (e.g., destroyed, picked up)
        public override void DeSpawn(DestroyMode mode)
        {
            Log.Message($"Building_TripWire: DeSpawn called for {LabelShort} (ID: {thingIDNumber}). Mode: {mode}.");
            Map map = Map; // Get map reference before base.DeSpawn potentially clears it
            base.DeSpawn(mode);
            if (map != null)
            {
                // Mark the area dirty when despawning to ensure the overlay is removed.
                map.mapDrawer.MapMeshDirty(Position, (ulong)TripWireMeshFlag.SecurityGrid);
                Log.Message($"Building_TripWire: Called MapMeshDirty during despawn for {LabelShort} at {Position} with SecurityGrid flag.");
            }
        }

        // This method is called by MapMeshDrawer when a specific SectionLayer needs to draw this Thing.
        // It's a key point for mesh-based drawing for 'MapMeshOnly' graphics.
        public override void Print(SectionLayer layer)
        {
            base.Print(layer); // Call base to ensure default printing for the ThingDef
            Log.Message($"Building_TripWire: Print called for {LabelShort} (ID: {thingIDNumber}) on layer {layer.GetType().Name}.");

            // Check if this Print call is specifically for our custom SecurityGrid layer.
            // We use GetType().Name to compare the layer type as a string at runtime,
            // which avoids potential compile-time reference issues for specific SectionLayer types.
            if (layer.GetType().Name == "SectionLayer_SecurityGrid")
            {
                Log.Message($"Building_TripWire: Print called by SectionLayer_SecurityGrid for {LabelShort} (ID: {thingIDNumber}). THIS IS GOOD!");
            }
            // Add checks for other layers if you want to see if it's drawing on anything else (e.g., regular buildings layer).
            else if (layer.GetType().Name == "SectionLayer_Buildings")
            {
                Log.Message($"Building_TripWire: Print called by SectionLayer_Buildings for {LabelShort} (ID: {thingIDNumber}).");
            }
        }

        // This is called when the Thing's color changes.
        public override void Notify_ColorChanged()
        {
            base.Notify_ColorChanged();
            Log.Message($"Building_TripWire: Notify_ColorChanged called for {LabelShort} (ID: {thingIDNumber}).");
            // This often triggers a MapMeshDirty on relevant layers.
            Map?.mapDrawer.MapMeshDirty(Position, (ulong)TripWireMeshFlag.SecurityGrid);
            Log.Message($"Building_TripWire: Called MapMeshDirty from Notify_ColorChanged for {LabelShort}.");
        }
    }
}