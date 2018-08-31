using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Milky
{
    /// <summary>
    /// Allows a humanoid to be milked, and milk themselves.
    /// </summary>
    public class CompMilkableHumanoid : ThingComp
    {
        /// <summary>
        /// Current milk progress in ticks.
        /// </summary>
        public int milkProgress;

        /// <summary>
        /// Milk properties.
        /// </summary>
        public CompProperties_MilkableHumanoid MilkProps
        {
            get
            {
                return props as CompProperties_MilkableHumanoid;
            }
        }

        /// <summary>
        /// Whether the pawn can actively be producing milk.
        /// </summary>
        public bool Active
        {
            get
            {
                Pawn pawn = parent as Pawn;

                if (MilkProps.onlyFemales)
                {
                    if (pawn.gender == Gender.Female)
                        return IsOfProperAge;
                    else
                        return false;
                }
                else if (MilkProps.onlyMales)
                {
                    if (pawn.gender == Gender.Male)
                        return IsOfProperAge;
                    else
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Is the pawn of proper age for milking?
        /// </summary>
        public bool IsOfProperAge
        {
            get
            {
                Pawn pawn = parent as Pawn;
                return pawn.ageTracker.AgeBiologicalYears >= MilkProps.minimumAgeToBeMilked;
            }
        }

        /// <summary>
        /// Whether the pawn can actively be milked.
        /// </summary>
        public bool ActiveAndCanBeMilked
        {
            get
            {
                return Active && CanBeMilked;
            }
        }

        /// <summary>
        /// Is the milk production done?
        /// </summary>
        public bool CanBeMilked
        {
            get
            {
                return milkProgress >= MilkProps.ticksUntilMilking;
            }
        }

        /// <summary>
        /// Milk production in percentage form.
        /// </summary>
        public float MilkProgressPercent
        {
            get
            {
                return (float)milkProgress / (float)MilkProps.ticksUntilMilking;
            }
        }

        /// <summary>
        /// Milk the pawn.
        /// </summary>
        /// <param name="milker">Milker, if any. Candidate for thoughts.</param>
        /// <returns>Milk thing.</returns>
        public Thing Milk(Pawn milker)
        {
            Pawn milkPawn = parent as Pawn;
            if (milkPawn == null)
                return null;

            Thing milk = ThingMaker.MakeThing(MilkProps.milkDef);
            milk.stackCount = MilkProps.milkAmount;

            if(milker == null)
            {
                if (MilkProps.milkThoughtMilkedSelf != null)
                    milkPawn.needs.mood.thoughts.memories.TryGainMemory(MilkProps.milkThoughtMilkedSelf);
            }
            else
            {
                if (MilkProps.milkThoughtMilker != null)
                    milker.needs.mood.thoughts.memories.TryGainMemory(MilkProps.milkThoughtMilker, milkPawn);

                if (MilkProps.milkThoughtMilked != null)
                    milkPawn.needs.mood.thoughts.memories.TryGainMemory(MilkProps.milkThoughtMilked, milker);
            }

            //Reset milk progress.
            milkProgress = 0;

            return milk;
        }

        /// <summary>
        /// Spawns the milk on the milker.
        /// </summary>
        /// <param name="milker">Milker who did the milking.</param>
        public void GatherMilk(Pawn milker)
        {
            Thing thingMilk = Milk(milker);
            if(thingMilk != null)
                GenSpawn.Spawn(thingMilk, milker.Position, milker.Map);
        }

        /// <summary>
        /// Spawns milk on themselves.
        /// </summary>
        public void GatherMilkSelf()
        {
            Thing thingMilk = Milk(null);
            if (thingMilk != null)
                GenSpawn.Spawn(thingMilk, parent.Position, parent.Map);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref milkProgress, "milkProgress");
        }

        public override string CompInspectStringExtra()
        {
            if (Active && (parent?.Faction.IsPlayer ?? false))
                return MilkProps.milkProgessKeyString.Translate(MilkProgressPercent.ToStringPercent());
            else
                return null;
        }

        public override void CompTick()
        {
            if(Active)
            {
                Pawn milkPawn = parent as Pawn;
                if (milkPawn == null)
                    return;

                bool canBeMilked = true;
                if (milkPawn.needs.food is Need_Food need && need != null && need.Starving)
                    canBeMilked = false;

                if (canBeMilked)
                {
                    if(milkProgress < MilkProps.ticksUntilMilking)
                        milkProgress++;
                }
            }
        }
    }
}
