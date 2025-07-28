using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TripWireAttempt2
{
    [HarmonyPatch(typeof(OverlayDrawer), nameof(OverlayDrawer.DrawAllOverlays))]
    public static class OverlayPatch
    {
        public static void Postfix()
        {
            if (!ShouldDrawOverlay()) return;

            Map map = Find.CurrentMap;
            if (map == null) return;

            var def = ThingDef.Named("TripWire");
            var ext = def.GetModExtension<ModExtension_TransmitterOverlay>();
            if (ext == null || string.IsNullOrEmpty(ext.transmitterAtlas))
            {
                return;
            }

            Texture2D tex = ContentFinder<Texture2D>.Get(ext.transmitterAtlas, false);
            if (tex == null)
            {
                Log.Warning("[TripWire] Could not load atlas texture.");
                return;
            }

            Material baseMat = MaterialPool.MatFrom(ext.transmitterAtlas, ShaderDatabase.MetaOverlay);
            baseMat.mainTexture.filterMode = FilterMode.Point;

            int tileSize = 80; // your tile size in pixels (make sure matches your texture!)
            int tilesX = tex.width / tileSize;
            int tilesY = tex.height / tileSize;

            foreach (Thing t in map.listerThings.ThingsOfDef(def))
            {
                IntVec3 pos = t.Position;

                // Check connections
                bool up = HasTripWireAt(pos + IntVec3.North, map);
                bool down = HasTripWireAt(pos + IntVec3.South, map);
                bool left = HasTripWireAt(pos + IntVec3.West, map);
                bool right = HasTripWireAt(pos + IntVec3.East, map);

                (int tileX, int tileY) = GetTileIndex(up, down, left, right);

                Vector2 uvScale = new Vector2(1f / tilesX, 1f / tilesY);
                int flippedY = (tilesY - 1) - tileY; // flip Y because UV origin is bottom-left
                Vector2 uvOffset = new Vector2(tileX * uvScale.x, flippedY * uvScale.y);

                // Clone material to set UV per tile
                Material mat = new Material(baseMat);
                mat.mainTextureScale = uvScale;
                mat.mainTextureOffset = uvOffset;

                Vector3 center = pos.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
                Graphics.DrawMesh(MeshPool.plane10, center, Quaternion.identity, mat, 0);
            }
        }

        private static bool HasTripWireAt(IntVec3 pos, Map map)
        {
            if (!pos.InBounds(map)) return false;
            var things = map.thingGrid.ThingsListAt(pos);
            return things.Exists(t => t.def.defName == "TripWire");
        }

        // Your tile picking logic here, simplified example based on your description
        private static (int x, int y) GetTileIndex(bool up, bool down, bool left, bool right)
        {
            if (left && down && !up && !right) return (0, 0);
            if (left && down && up && !right) return (1, 0);
            if (left && down && !up && right) return (2, 0);
            if (left && down && up && right) return (3, 0);

            if (left && !down && !up && !right) return (0, 1);
            if (left && !down && up && !right) return (1, 1);
            if (left && !down && !up && right) return (2, 1);
            if (left && !down && up && right) return (3, 1);

            if (!left && down && !up && !right) return (0, 2);
            if (!left && down && up && !right) return (1, 2);
            if (!left && down && !up && right) return (2, 2);
            if (!left && down && up && right) return (3, 2);

            if (!left && !down && !up && !right) return (0, 3);
            if (!left && !down && up && !right) return (1, 3);
            if (!left && !down && !up && right) return (2, 3);
            if (!left && !down && up && right) return (3, 3);

            // No connections
            return (0, 3); // isolated or fallback tile
        }


        private static bool ShouldDrawOverlay()
        {
            var designator = Find.DesignatorManager?.SelectedDesignator;
            return designator is Designator_Build build && build.PlacingDef?.defName == "TripWire";
        }
    }
}