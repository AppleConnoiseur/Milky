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
    /// Drives the Job when milking themselves.
    /// </summary>
    public class JobDriver_MilkSelf : JobDriver
    {
        protected float WorkTotal
        {
            get
            {
                return 600f;
            }
        }

        private float gatherProgress;
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil wait = new Toil();

            wait.tickAction = delegate
            {
                Pawn actor = wait.actor;
                this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
                if (this.gatherProgress >= this.WorkTotal)
                {
                    Pawn milkedPawn = actor;
                    milkedPawn.TryGetComp<CompMilkableHumanoid>().GatherMilkSelf();
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
                }
            };

            wait.AddEndCondition(delegate
            {
                Pawn milkedPawn = wait.actor;
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
