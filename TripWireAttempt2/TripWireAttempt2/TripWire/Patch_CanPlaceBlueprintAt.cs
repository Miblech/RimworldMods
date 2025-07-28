using HarmonyLib;
using RimWorld;
using Verse;

namespace TrapsExpanded
{
    [HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.CanPlaceBlueprintAt))]
    public static class Patch_CanPlaceBlueprintAt
    {
        public static void Postfix(
            BuildableDef entDef,
            IntVec3 center,
            Rot4 rot,
            Map map,
            bool godMode,
            Thing thingToIgnore,
            Thing thing,
            ThingDef stuffDef,
            bool ignoreEdgeArea,
            bool ignoreInteractionSpots,
            bool ignoreClearableFreeBuildings,
            ref AcceptanceReport __result)
        {
            if (!__result.Accepted) return;

            if (entDef.defName != "TrapsExpanded") return;

            var things = center.GetThingList(map);
            foreach (Thing t in things)
            {
                if (t.def.defName.StartsWith("TrapIED_"))
                {
                    __result = new AcceptanceReport("Cannot place TrapsExpanded on top of IED.");
                    return;
                }
            }
        }
    }
}