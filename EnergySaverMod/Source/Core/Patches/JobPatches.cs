using EnergySaverMod.Source.Core.Container;
using EnergySaverMod.Source.Core.Helper;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(Pawn_JobTracker), nameof(Pawn_JobTracker.StartJob))]
public static class Pawn_JobTracker_Pawn_JobTracker_StartJob
{
	public static void Postfix(Pawn_JobTracker __instance)
	{
		if (__instance.curJob?.def == JobDefOf.DoBill || __instance.curJob?.def == JobDefOf.Research)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance.curJob?.targetA.Thing);
			
			if(flickable != null && PatchesHelper.ShouldSwitchPower(__instance.curJob?.targetA.Thing))
			{
				PatchesHelper.SetSwitch(flickable, true);
			}
			
			FacilityHelper.SetLinkedMultiAnalyzerSwitch(__instance.curJob?.targetA.Thing as Building_ResearchBench, true);
		}
		else if (__instance.curJob?.def == JobDefOf.AnalyzeItem)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance.curJob?.targetB.Thing);
			
			if(flickable != null && PatchesHelper.ShouldSwitchPower(__instance.curJob?.targetB.Thing))
			{
				PatchesHelper.SetSwitch(flickable, true);
			}

			FacilityHelper.SetLinkedMultiAnalyzerSwitch(__instance.curJob?.targetB.Thing as Building_ResearchBench, true);
		}
	}
}

[HarmonyPatch(typeof(Pawn_JobTracker), nameof(Pawn_JobTracker.EndCurrentJob))]
public static class Pawn_JobTracker_Pawn_JobTracker_EndCurrentJob
{
	public static bool Prefix(Pawn_JobTracker __instance, JobCondition condition, bool startNewJob = true, bool canReturnToPool = true)
	{
		if(__instance.curJob?.targetA.Thing is Building_MechGestator mechGestator)
		 {
		 	if (mechGestator.ActiveMechBill != null)
		 	{
		 		return true;
		 	}
		 }
		
		if (__instance.curJob?.def == JobDefOf.DoBill || __instance.curJob?.def == JobDefOf.Research)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance.curJob?.targetA.Thing);
			if(flickable != null && PatchesHelper.ShouldSwitchPower(__instance.curJob?.targetA.Thing))
			{
				PatchesHelper.SetSwitch(flickable, false);
			}
			
			FacilityHelper.SetLinkedMultiAnalyzerSwitch(__instance.curJob?.targetA.Thing as Building_ResearchBench, false);
		}
		else if (__instance.curJob?.def == JobDefOf.AnalyzeItem)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance.curJob?.targetB.Thing);
			
			if(flickable != null && PatchesHelper.ShouldSwitchPower(__instance.curJob?.targetB.Thing))
			{
				PatchesHelper.SetSwitch(flickable, false);
			}
			
			FacilityHelper.SetLinkedMultiAnalyzerSwitch(__instance.curJob?.targetB.Thing as Building_ResearchBench, false);
		}

		return true;
	}
}