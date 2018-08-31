using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Milky
{
    /// <summary>
    /// Properties for the milk component.
    /// </summary>
    public class CompProperties_MilkableHumanoid : CompProperties
    {
        /// <summary>
        /// ThingDef for the milk.
        /// </summary>
        public ThingDef milkDef;

        /// <summary>
        /// Amount of milk to spawn.
        /// </summary>
        public int milkAmount = 15;

        /// <summary>
        /// Thought given to the milker.
        /// </summary>
        public ThoughtDef milkThoughtMilker;

        /// <summary>
        /// Thought given to the milked.
        /// </summary>
        public ThoughtDef milkThoughtMilked;

        /// <summary>
        /// Thought given when pawn milked themselves.
        /// </summary>
        public ThoughtDef milkThoughtMilkedSelf;

        /// <summary>
        /// Ticks until the pawn can be milked.
        /// </summary>
        public int ticksUntilMilking = 60000;

        /// <summary>
        /// Can only females be milked?
        /// </summary>
        public bool onlyFemales = true;

        /// <summary>
        /// Can only males be milked?
        /// </summary>
        public bool onlyMales = false;

        /// <summary>
        /// Whether the pawn can milk themselves.
        /// </summary>
        public bool canMilkThemselves = true;

        /// <summary>
        /// Jobs that milking simply can't interrupt. E.g first aid and surgery.
        /// </summary>
        public List<JobDef> forbiddenJobsToInterrupt = new List<JobDef>();

        /// <summary>
        /// Minimum age before the pawn can be milked.
        /// </summary>
        public int minimumAgeToBeMilked = 0; //New

        /// <summary>
        /// Text shown in the inspector when they are eligible to be milked.
        /// </summary>
        public string milkProgessKeyString = "MilkyHumanoidMilkProgress"; //New

        public CompProperties_MilkableHumanoid()
        {
            compClass = typeof(CompMilkableHumanoid);
        }
    }
}
