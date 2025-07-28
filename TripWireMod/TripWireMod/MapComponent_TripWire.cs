using Verse;
using RimWorld;
using UnityEngine;

namespace TripWireTrap
{
    public class MapComponent_TripWire : MapComponent
    {
        public static bool SecurityGridActive = false;

        private bool _securityGridMapInitialized = false;

        public MapComponent_TripWire(Map map) : base(map)
        {
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            if (!_securityGridMapInitialized && map.mapDrawer != null && Current.ProgramState == ProgramState.Playing)
            {
                Log.Message($"MapComponent_TripWire: Initializing security grid display. Calling WholeMapChanged with SecurityGrid flag ({(ulong)TripWireMeshFlag.SecurityGrid}).");
                map.mapDrawer.WholeMapChanged((ulong)TripWireMeshFlag.SecurityGrid);
                _securityGridMapInitialized = true;
            }

            if (Find.TickManager.TicksGame % 10 != 0)
            {
                return;
            }

            bool newSecurityGridActiveState = false;

            if (Find.DesignatorManager.SelectedDesignator is Designator_Build designatorBuild &&
                designatorBuild.PlacingDef is ThingDef thingDefBeingBuilt &&
                thingDefBeingBuilt.defName == "TripWireTrap")
            {
                newSecurityGridActiveState = true;
            }
            else if (Find.Selector.SelectedObjects.Any(o => o is Building_TripWire))
            {
                newSecurityGridActiveState = true;
            }

            if (SecurityGridActive != newSecurityGridActiveState)
            {
                if (Current.ProgramState == ProgramState.Playing)
                {
                    Log.Message($"MapComponent_TripWire: SecurityGridActive changing from {SecurityGridActive} to {newSecurityGridActiveState}.");
                }
                SecurityGridActive = newSecurityGridActiveState;

                if (Current.ProgramState == ProgramState.Playing)
                {
                    Log.Message($"MapComponent_TripWire: Calling WholeMapChanged with SecurityGrid flag ({(ulong)TripWireMeshFlag.SecurityGrid}).");
                }
                if (map.mapDrawer != null)
                {
                    map.mapDrawer.WholeMapChanged((ulong)TripWireMeshFlag.SecurityGrid);
                }
            }
        }

        public override void MapGenerated()
        {
            base.MapGenerated();
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }

    }
}