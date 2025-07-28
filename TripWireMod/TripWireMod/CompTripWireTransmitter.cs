using Verse;
using System.Collections.Generic; // Required for List

namespace TripWireTrap
{
    public class CompProperties_TripWireTransmitter : CompProperties
    {
        public CompProperties_TripWireTransmitter()
        {
            this.compClass = typeof(CompTripWireTransmitter);
        }
    }

    public class CompTripWireTransmitter : ThingComp
    {
        public List<CompTripWireConnector> connectChildren = new List<CompTripWireConnector>(); // List of connectors this transmitter links to

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Log.Message($"CompTripWireTransmitter: PostSpawnSetup called for {parent.LabelShort}.");
        }

        public void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            Log.Message($"CompTripWireTransmitter: PostDeSpawn called for {parent.LabelShort}.");
        }
    }
}