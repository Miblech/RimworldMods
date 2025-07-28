using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;

[HarmonyPatch(typeof(PawnGenerator))]
[HarmonyPatch("GenerateRandomAge")]
public static class Patch_GenerateRandomAge
{
    static void Postfix(Pawn pawn, PawnGenerationRequest request)
    {

        if (request.FixedBiologicalAge.HasValue)
            return;

        if (pawn.kindDef.minGenerationAge < 19f)
            return;

        if (pawn.RaceProps.Animal || pawn.RaceProps.IsMechanoid || pawn.IsBloodfeeder()) return;

        float meanAge = 30f;
        float stdDev = 7f;


        float newAge;
        int attempts = 0;
        do
        {
            newAge = (float)(RandomNormal(meanAge, stdDev));
            attempts++;
            if (attempts > 100) break;
        }
        while (newAge < 15f || newAge > 60f);


        long ageTicks = (long)(newAge * 3600000f);
        pawn.ageTracker.AgeBiologicalTicks = ageTicks;
        pawn.ageTracker.AgeChronologicalTicks = ageTicks;


        int ticksAbs = GenTicks.TicksAbs;
        pawn.ageTracker.BirthAbsTicks = (long)ticksAbs - ageTicks;
        pawn.ageTracker.AgeChronologicalTicks = ageTicks;
    }


    private static double RandomNormal(double mean, double stddev)
    {

        double u1 = 1.0 - Rand.Value;
        double u2 = 1.0 - Rand.Value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                               Math.Sin(2.0 * Math.PI * u2);
        return mean + stddev * randStdNormal;
    }
}