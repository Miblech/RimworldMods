using HarmonyLib;
using Verse;

namespace TrapsExpanded
{
    
        public class ModInitializer : Mod
        {
            public ModInitializer(ModContentPack content) : base(content)
            {
                var harmony = new Harmony("MPCatcher.TrapsExpanded");
                harmony.PatchAll();
            }
        }
    
}