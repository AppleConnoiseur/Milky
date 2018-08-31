using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Milky
{
    /// <summary>
    /// Deprecated class used during development.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class PostPatch
    {
        static PostPatch()
        {
            //Manually patch in Milking
            /*ThingDef orassanThingDef = ThingDef.Named("Alien_Orassan");

            Log.Message("Patching '" + orassanThingDef?.LabelCap + "' milking in.");

            if(orassanThingDef != null)
            {
                Log.Message("-=Comps Before=-");
                foreach(CompProperties thingCompProps in orassanThingDef.comps)
                {
                    Log.Message(thingCompProps.GetType().FullName + ": compClass='" + thingCompProps?.compClass?.FullName + "'");
                }

                CompProperties_Milkable compProps = new CompProperties_Milkable();
                compProps.milkDef = ThingDef.Named("Milk");
                compProps.milkIntervalDays = 1;
                compProps.milkAmount = 15;

                orassanThingDef.comps.Add(compProps);

                Log.Message("Patching completed.");

                Log.Message("-=Comps After=-");
                foreach (CompProperties thingCompProps in orassanThingDef.comps)
                {
                    Log.Message(thingCompProps.GetType().FullName + ": compClass='" + thingCompProps?.compClass?.FullName + "'");
                }
            }*/
        }
    }
}
