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
		if (__instance.curJob?.def == JobDefOf.Research)
		{
			Log.Message($"Log{__instance.curJob?.ToString()}");
		}
		
		if (__instance.curJob?.def == JobDefOf.DoBill || __instance.curJob?.def == JobDefOf.Research)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance.curJob?.targetA.Thing);
			if(flickable != null && !PatchesHelper.CanWorkWithoutPower(__instance.curJob?.targetA.Thing))
			{
				Log.Message("flickableStart " + flickable);
				flickable.SwitchIsOn = true;
				flickable.WantsFlick();
			}
		}
	}
}

[HarmonyPatch(typeof(Pawn_JobTracker), nameof(Pawn_JobTracker.EndCurrentJob))]
public static class Pawn_JobTracker_Pawn_JobTracker_EndCurrentJob
{
	public static bool Prefix(Pawn_JobTracker __instance, JobCondition condition, bool startNewJob = true, bool canReturnToPool = true)
	{
		if (__instance.curJob?.def == JobDefOf.DoBill || __instance.curJob?.def == JobDefOf.Research)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance.curJob?.targetA.Thing);
			
			if(flickable != null && !PatchesHelper.CanWorkWithoutPower(__instance.curJob?.targetA.Thing))
			{
				Log.Message("flickableEnd " + flickable);
				flickable.SwitchIsOn = false;
				flickable.WantsFlick();
			}
		}

		return true;
	}
}