using HarmonyLib;
using Verse;

public class PawnAgeMod : Mod
{
    public PawnAgeMod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("com.yourname.pawnagemod");
        harmony.PatchAll();
    }
}