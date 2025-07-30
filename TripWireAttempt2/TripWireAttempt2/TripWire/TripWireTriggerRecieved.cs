using Verse;

namespace TrapsExpanded
{
    public interface ITripWireReceiver
    {

        void OnTripWireActivated(Pawn p, Thing tripWire);

    }
}
