using EnergySaverMod.Source.Core.Container;
using EnergySaverMod.Source.Core.Helper;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(JobDriver), "Cleanup")]
public static class JobDriver_Cleanup_Patches
{
	public static void Postfix(JobDriver __instance, JobCondition condition)
	{
		if (__instance?.job != null && 
		    (__instance.job.def == JobDefOf.DoBill || __instance.job.def == JobDefOf.Research))
		{
			Thing targetThing = __instance.job.targetA.Thing;
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(targetThing);
            
			if(flickable != null && PatchesHelper.ShouldSwitchPower(targetThing))
			{
				PatchesHelper.SetSwitch(flickable, false);
			}
            
			FacilityHelper.SetLinkedMultiAnalyzerSwitch(targetThing as Building_ResearchBench, false);
		}
		else if (__instance?.job?.def == JobDefOf.AnalyzeItem)
		{
			Thing targetThing = __instance.job.targetB.Thing;
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(targetThing);
            
			if(flickable != null && PatchesHelper.ShouldSwitchPower(targetThing))
			{
				PatchesHelper.SetSwitch(flickable, false);
			}
            
			FacilityHelper.SetLinkedMultiAnalyzerSwitch(targetThing as Building_ResearchBench, false);
		}
	}
}