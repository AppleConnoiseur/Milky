using RimWorld;
using Verse.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Milky
{
    /// <summary>
    /// Gives work to colonists to milk milkable colonists.
    /// </summary>
    public class WorkGiver_MilkHumanoid : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            List<Pawn> pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
            for (int i = 0; i < pawns.Count; i++)
            {
                yield return pawns[i];
            }
            yield break;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn targetPawn = t as Pawn;

            if (targetPawn == null)
                return false;

            Pawn lover = LovePartnerRelationUtility.ExistingLovePartner(pawn);
            if (lover == null)
                return false;

            if (pawn.Faction != targetPawn.Faction)
                return false;

            if (lover != targetPawn)
                return false;

            if (lover.Drafted)
                return false;

            //Do not try to milk sleeping lovers. Its rude.
            if (lover.CurJob != null && lover.jobs.curDriver.asleep)
                return false;

            //Do not intervene in player forced jobs.
            if (lover.CurJob != null && lover.CurJob.playerForced)
                return false;

            CompMilkableHumanoid spouseMilkComp = lover.TryGetComp<CompMilkableHumanoid>();
            if (spouseMilkComp == null)
                return false;

            if (!spouseMilkComp.ActiveAndCanBeMilked)
                return false;

            //Do not interrupt forbidden jobs.
            if(lover.CurJob != null && spouseMilkComp.MilkProps.forbiddenJobsToInterrupt.Count > 0 && 
                spouseMilkComp.MilkProps.forbiddenJobsToInterrupt.Contains(lover.CurJob.def))
                return false;

            if (lover.Position.IsForbidden(pawn))
                return false;

            if (!pawn.CanReserve(new LocalTargetInfo(lover)))
                return false;

            if (!pawn.CanReach(new LocalTargetInfo(lover), PathEndMode.ClosestTouch, Danger.Deadly))
                return false;

            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(MilkyDefOf.MilkyLover, new LocalTargetInfo(t));
        }
    }
}
