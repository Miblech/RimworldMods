using RimWorld;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;

namespace TrapsExpanded
{
    public class Building_ShotgunTrap : Building_Trap
    {
        private int remainingShots;
        private const int MaxInitialShots = 5;

        public Building_ShotgunTrap()
        {
            remainingShots = MaxInitialShots;
        }

        protected override void SpringSub(Pawn p)
        {
            if (remainingShots > 0)
            {

                SoundDef.Named("Shot_Shotgun_Trap").PlayOneShot(new TargetInfo(Position, Map));

                int shotsToFire = Rand.Range(1, remainingShots + 1);
                remainingShots -= shotsToFire;

                for (int i = 0; i < shotsToFire; i++)
                {
                    DealShotgunProjectileDamage(p);
                }

                Messages.Message($"Trap fired {shotsToFire} buckshot pellets at {p.Name}. Out of: {MaxInitialShots}.", MessageTypeDefOf.PositiveEvent);

            }
        }

        private BodyPartRecord SelectShotgunTrapHitPart(Pawn p)
        {
            var parts = p.health.hediffSet.GetNotMissingParts()
                .Where(part => part.def.tags.Contains(BodyPartTagDefOf.Pelvis) || part.def.tags.Contains(BodyPartTagDefOf.MovingLimbSegment)).ToList();

            if (!parts.Any())
                return null;

            return parts.RandomElementByWeight(part => part.coverage);
        }

        private void DealShotgunProjectileDamage(Pawn target)
        {
            const float damage = 18f;
            const float armorPen = 0.14f;

            BodyPartRecord hitPart = SelectShotgunTrapHitPart(target);

            DamageInfo dinfo = new DamageInfo(
                DamageDefOf.Bullet,
                damage,
                armorPen,
                angle: 0f,
                instigator: this,
                hitPart: hitPart,
                weapon: null,
                category: DamageInfo.SourceCategory.ThingOrUnknown
            );

            target.TakeDamage(dinfo);

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref remainingShots, "remainingShots", MaxInitialShots);
        }

        public override string GetInspectString()
        {
            var sb = new StringBuilder();
            var baseStr = base.GetInspectString();
            if (!string.IsNullOrEmpty(baseStr))
                sb.AppendLine(baseStr);

            sb.Append($"Remaining Shots: {remainingShots}/{MaxInitialShots}");
            return sb.ToString();
        }
    }
}
