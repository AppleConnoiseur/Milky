using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Milky
{
    /// <summary>
    /// Gives jobs to pawns that can be milked to milk themselves.
    /// </summary>
    public class JobGiver_MilkSelf : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.AnimalOrWildMan())
                return null;

            if (!pawn.IsColonist)
                return null;

            if (pawn.Drafted)
                return null;

            if (pawn.Downed)
                return null;

            if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
                return null;

            CompMilkableHumanoid compMilk = pawn.TryGetComp<CompMilkableHumanoid>();

            if (compMilk == null)
                return null;

            if (!compMilk.ActiveAndCanBeMilked)
                return null;

            if (!compMilk.MilkProps.canMilkThemselves)
                return null;

            Pawn lover = LovePartnerRelationUtility.ExistingLovePartner(pawn);
            if (lover != null)
            {
                if (pawn.Faction == lover.Faction)
                {
                    if (!lover.Drafted && !lover.Downed && !HealthAIUtility.ShouldSeekMedicalRest(lover))
                        return null;
                }
            }

            return new Job(MilkyDefOf.MilkySelf);
        }
    }
}
