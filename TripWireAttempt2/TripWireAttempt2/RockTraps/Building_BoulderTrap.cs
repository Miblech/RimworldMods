using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace TrapsExpanded
{
    public class Building_BoulderTrap : Building_Trap
    {

        protected override void SpringSub(Pawn p)
        {

            ThingDef chunkDef = this.Stuff;
            Thing boulder = ThingMaker.MakeThing(chunkDef);
            GenPlace.TryPlaceThing(boulder, this.Position, this.Map, ThingPlaceMode.Direct);
            SoundDef.Named("BoulderTrap_Spring").PlayOneShot(new TargetInfo(Position, Map));

            if (p != null && !p.Dead)
            {
                BodyPartRecord targetPart = GetWeightedHitPart(p);

                DamageInfo dinfo = new DamageInfo(DamageDefOf.Blunt, 30f, 999f, -1f, this, targetPart);
                p.TakeDamage(dinfo);

                // Optional: log it
                Log.Message($"[BoulderTrap] {p.LabelShort} got crushed by a boulder on their {targetPart?.Label ?? "body"}!");
            }

            // Self-destroy the trap
            this.Destroy();
        }

        private BodyPartRecord GetWeightedHitPart(Pawn pawn)
        {
            if (pawn.RaceProps.body == null || pawn.health.hediffSet == null)
                return null;

            List<BodyPartRecord> parts = pawn.RaceProps.body.AllParts;
            List<BodyPartRecord> candidateParts = new List<BodyPartRecord>();

            foreach (var part in parts)
            {
                if (!pawn.health.hediffSet.PartIsMissing(part))
                {
                    if (part.def == BodyPartDefOf.Head || part.def.defName.Contains("Shoulder"))
                    {
                        // 2/3 chance
                        if (Rand.Value < 0.67f)
                            return part;
                    }
                    else
                    {
                        candidateParts.Add(part);
                    }
                }
            }

            // Fall back to random part
            return candidateParts.RandomElementWithFallback();
        }
    }
}