using HarmonyLib;
using Verse;
using RimWorld;
using System.Reflection;

namespace TripWireTrap
{
    // This class will contain your Harmony patches related to MapDrawer.
    [HarmonyPatch(typeof(MapDrawer))] // Target the MapDrawer class for patching
    public static class MapDrawer_Patch
    {
        // Patch the DrawMapMesh method to inject our custom layer drawing.
        // We use a Postfix to ensure our layer draws *after* the vanilla layers.
        // __instance refers to the instance of the MapDrawer class being patched.
        [HarmonyPostfix]
        [HarmonyPatch("DrawMapMesh")] // Patch the method named "DrawMapMesh"
        public static void Postfix_DrawMapMesh(MapDrawer __instance)
        {
            // Only draw our custom layer if the SecurityGridActive flag is true.
            if (!MapComponent_TripWire.SecurityGridActive)
            {
                return;
            }

            // Get the current Map from the MapDrawer instance.
            // MapDrawer's 'map' field is private, so we access it via reflection.
            // This is a common pattern for accessing private fields/methods in Harmony patches.
            Map map = (Map)typeof(MapDrawer).GetField("map", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

            if (map == null) return;

            // Get our custom SectionLayer_SecurityGrid instance for this map.
            // GetGlobalLayer will create it if it doesn't exist for the map yet.
            TripWireTrap.SectionLayer_SecurityGrid securityLayer = map.mapDrawer.GetGlobalLayer<TripWireTrap.SectionLayer_SecurityGrid>();

            if (securityLayer == null)
            {
                Log.ErrorOnce("TripWireTrap: SectionLayer_SecurityGrid not found or could not be created in MapDrawer's global layers. This is a critical error.", 987654321);
                return;
            }

            // If our layer needs to be regenerated (because we marked it dirty with TripWireMeshFlag.SecurityGrid)
            if (securityLayer.Dirty)
            {
                Log.Message("TripWireTrap: Regenerating SectionLayer_SecurityGrid.");
                securityLayer.Regenerate(); // Call our custom layer's Regenerate method
                securityLayer.RefreshSubMeshBounds(); // Refresh bounds after regenerating to update drawing area
            }

            // Draw our custom layer.
            // SectionLayer.Visible is typically true for layers that should be drawn.
            if (securityLayer.Visible)
            {
                Log.Message("TripWireTrap: Drawing SectionLayer_SecurityGrid.");
                securityLayer.DrawLayer();
            }
        }
    }
}