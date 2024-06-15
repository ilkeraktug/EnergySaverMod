using System.Reflection;
using EnergySaverMod.Source.Core.Helper;
using EnergySaverMod.Source.Core.Mod;
using HarmonyLib;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(Building), nameof(Building.SpawnSetup))]
public static class Building_ResearchBench_SpawnSetup_Patches
{
	public static void Postfix(Building __instance, Map map, bool respawningAfterLoad)
	{
		if ((__instance is Building_ResearchBench || __instance is Building_WorkTable) && respawningAfterLoad == false)
		{
			CompFlickable flickable = GetComponentHelper.GetFlickableComponent(__instance);
			if (EnerySaverModSettings.bShouldSpawnUnpowered && flickable != null)
			{
				// bool myBool = false;
				// FieldInfo wantSwitchOnFieldInfo = AccessTools.Field(typeof(CompFlickable), "wantSwitchOn");
				// wantSwitchOnFieldInfo.SetValue(flickable, myBool);
				//
				// flickable.SwitchIsOn = false;

				PatchesHelper.SetSwitch(flickable, false);
			}
		}
	}
}

[HarmonyPatch(typeof(ResearchProjectDef), nameof(ResearchProjectDef.CanBeResearchedAt))]
public static class ResearchProjectDef_CanBeResearchedAt_Patches
{
	public static void Postfix(ref bool __result, ResearchProjectDef __instance, Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
	{
		bool shouldCare = false;
		bool tempResult = PatchesHelper.CanUseNow(bench, ref shouldCare);
		if (!ignoreResearchBenchPowerStatus && shouldCare)
		{
			//Log.Message($"CanBeResearchedAt({tempResult})");
			__result = tempResult;
		}
	}
}

[HarmonyPatch(typeof(Building_WorkTable), nameof(Building_WorkTable.UsableForBillsAfterFueling))]
public static class Building_WorkTable_UsableForBillsAfterFueling_Patches
{
	public static void Postfix(ref bool __result, Building_WorkTable __instance)
	{
		bool shouldCare = false;
		bool tempResult = PatchesHelper.CanUseNow(__instance, ref shouldCare);
		if (shouldCare)
		{
			__result = tempResult;
		}
	}
}

[HarmonyPatch(typeof(Building_WorkTable), nameof(Building_WorkTable.CurrentlyUsableForBills))]
public static class Building_WorkTable_CurrentlyUsableForBills_Patches
{
	public static void Postfix(ref bool __result, Building_WorkTable __instance)
	{
		bool shouldCare = false;
		bool tempResult = PatchesHelper.CanUseNow(__instance, ref shouldCare);
		if (shouldCare)
		{
			__result = tempResult;
		}
	}
}
