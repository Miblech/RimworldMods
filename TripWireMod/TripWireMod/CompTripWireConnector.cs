using System.Collections.Generic; // Required for List
using Verse;

namespace TripWireTrap
{
    public class CompProperties_TripWireConnector : CompProperties
    {
        public CompProperties_TripWireConnector()
        {
            this.compClass = typeof(CompTripWireConnector);
        }
    }

    public class CompTripWireConnector : ThingComp
    {
        public CompTripWireTransmitter connectParent; // The transmitter this connector is connected to

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Log.Message($"CompTripWireConnector: PostSpawnSetup called for {parent.LabelShort}.");
        }

        public void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            Log.Message($"CompTripWireConnector: PostDeSpawn called for {parent.LabelShort}.");
        }
    }
}