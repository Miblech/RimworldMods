using RimWorld;
using System.Linq;
using Verse;
using Verse.Sound;

namespace TrapsExpanded
{
    public class Building_LandSlideTrap : Building_Trap
    {
            protected override void SpringSub(Pawn p)
        {
            SoundDef.Named("BoulderTrap_Spring").PlayOneShot(new TargetInfo(Position, Map));

            IntVec3 pawnCell = (p != null) ? p.Position : Position;
            float pawnMass = (p != null) ? p.def.BaseMass : 0f;

            string logMessage = "[LandslideTrap] ";
            if (p == null)
            {
                logMessage += $"Landslide sprung at {Position}. No pawn found.";
            }
            else if (p.Dead)
            {
                logMessage += $"Landslide sprung at {Position}. {p.LabelShort} was already dead.";
            }
            else if (pawnMass < 80f)
            {
                p.Destroy();
                logMessage += $"{p.LabelShort} (Mass: {pawnMass:F1}kg) was instantly crushed to death with no body remaining at {pawnCell}!";
            }
            else
            {
                float damage = (pawnMass < 180f) ? 120f : 40f;
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, damage, instigator: this);
                p.TakeDamage(dinfo);
                logMessage += $"{p.LabelShort} (Mass: {pawnMass:F1}kg) took {damage:F1} crushing damage at {pawnCell}!";
            }

            Log.Message(logMessage);



            if (this.Map != null)
            {
                IntVec3 wallCell = TrapUtils.DetermineWallCell(this.Position, this.Rotation);
                if (wallCell.InBounds(this.Map))
                {
                    foreach (Thing t in wallCell.GetThingList(this.Map).ToList())
                    {
                        if (t.def.building != null || t.def.defName.StartsWith("Mineable"))
                        {
                            t.Destroy(DestroyMode.KillFinalize);
                            Log.Message($"[LandslideTrap] Destroyed thing at wall cell: {t.LabelCap} (defName: {t.def.defName})");
                        }
                    }
                }
                else
                {
                    Log.Warning($"[LandslideTrap] Wall cell {wallCell} is out of bounds for map.");
                }
            }
            else
            {
                Log.Warning("[LandslideTrap] Map is null during SpringSub (before wall destruction).");
            }

            if (this.Map != null)
            {
                ThingDef rockDef = ThingDef.Named("CollapsedRocks");
                Thing rocks = ThingMaker.MakeThing(rockDef);
                IntVec3 placeRockCell = (p != null && !p.Dead) ? p.Position : Position;
                GenPlace.TryPlaceThing(rocks, placeRockCell, this.Map, ThingPlaceMode.Direct);
            }
            else
            {
                Log.Warning("[LandslideTrap] Map is null during SpringSub (before placing rocks).");
            }
        }
    }
}