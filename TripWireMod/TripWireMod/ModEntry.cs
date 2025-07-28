using HarmonyLib; // Required for Harmony
using Verse;     // Required for Log

namespace TripWireTrap
{
    // [StaticConstructorOnStartup] tells RimWorld to run this static constructor when the game starts up.
    [StaticConstructorOnStartup]
    public static class ModEntry
    {
        static ModEntry()
        {
            // Create a Harmony instance with a unique ID for your mod.
            // It's good practice to use your mod's package ID (e.g., "YourModAuthor.YourModName").
            // Make sure this ID is unique across all your mods to prevent conflicts.
            var harmony = new Harmony("TripWireTrap.mod");

            // Apply all Harmony patches defined in your assembly (i.e., in this mod's C# code).
            harmony.PatchAll();

            Log.Message("TripWireTrap: Harmony patches applied successfully!");
        }
    }
}