using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TripWireMod
{
    [DefOf]
    public static class TripWireTrapDefOf
    {

        public static ThingDef TripWireTrap;

        static TripWireTrapDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TripWireTrapDefOf));
        }
    }
}