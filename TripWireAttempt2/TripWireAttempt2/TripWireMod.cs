using HarmonyLib;
using Verse;

namespace TripWireAttempt2
{
    
        public class ModInitializer : Mod
        {
            public ModInitializer(ModContentPack content) : base(content)
            {
                var harmony = new Harmony("MPCatcher.TripWire");
                harmony.PatchAll();
            }
        }
    
}