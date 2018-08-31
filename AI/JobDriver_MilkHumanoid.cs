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
    /// Drives the Job when milking a humanoid.
    /// </summary>
    public class JobDriver_MilkHumanoid : JobDriver
    {
        protected float WorkTotal
        {
            get
            {
                return 400f;
            }
        }

        private float gatherProgress;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnDowned(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            Toil wait = new Toil();
            wait.initAction = delegate
            {
                Pawn actor = wait.actor;
                Pawn pawn = (Pawn)wait.actor.CurJob.GetTarget(TargetIndex.A).Thing;
                actor.pather.StopDead();
                PawnUtility.ForceWait(pawn, 15000, null, true);
            };
            wait.tickAction = delegate
            {
                Pawn actor = wait.actor;
                actor.skills.Learn(SkillDefOf.Social, 0.142999992f, false);
                this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
                if (this.gatherProgress >= this.WorkTotal)
                {
                    Pawn milkedPawn = (Pawn)((Thing)this.job.GetTarget(TargetIndex.A));
                    milkedPawn.TryGetComp<CompMilkableHumanoid>().GatherMilk(this.pawn);
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
                }
            };
            wait.AddFinishAction(delegate
            {
                Pawn pawn = (Pawn)wait.actor.CurJob.GetTarget(TargetIndex.A).Thing;
                if (pawn.jobs.curJob.def == JobDefOf.Wait_MaintainPosture)
                {
                    pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
                }
            });
            wait.FailOnDespawnedOrNull(TargetIndex.A);
            wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            wait.AddEndCondition(delegate
            {
                Pawn milkedPawn = (Pawn)((Thing)this.job.GetTarget(TargetIndex.A));
                if (!milkedPawn.TryGetComp<CompMilkableHumanoid>().ActiveAndCanBeMilked)
                {
                    return JobCondition.Incompletable;
                }
                return JobCondition.Ongoing;
            });
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f);
            yield return wait;
        }
    }
}
