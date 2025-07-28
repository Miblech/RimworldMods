using System;
using UnityEngine;
using Verse;

namespace TripWireTrap
{
    public class TripWireNetDef : Def
    {
        public string overlayTexPath;

        private Material overlayMaterial;

        public Material OverlayMaterial
        {
            get
            {
                if (overlayMaterial == null)
                {
                    LongEventHandler.ExecuteWhenFinished(delegate
                    {
                        if (overlayTexPath.NullOrEmpty())
                        {
                            Log.Error("TripWireNetDef (" + defName + ") has null or empty overlayTexPath. Using default yellow.");
                            // Fallback to a default yellow material if path is invalid
                            overlayMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.yellow, ShaderDatabase.MetaOverlay);
                            return;
                        }
                        try
                        {
                            // Attempt to load the specified material
                            overlayMaterial = MaterialPool.MatFrom(overlayTexPath, ShaderDatabase.MetaOverlay, Color.white);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"TripWireNetDef ({defName}): Failed to load overlay material from path '{overlayTexPath}'. Using default yellow. Exception: {ex.Message}");
                            // Fallback to a default yellow material on load failure
                            overlayMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.yellow, ShaderDatabase.MetaOverlay);
                        }
                    });
                }
                return overlayMaterial;
            }
        }
    }
}