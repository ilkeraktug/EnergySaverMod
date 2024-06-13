using EnergySaverMod.Source.Core.Helper;
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
			if (flickable != null)
			{
				Log.Message("SpawnSetup " + flickable);
				flickable.SwitchIsOn = false;
				flickable.WantsFlick();
			}
		}
	}
}

[HarmonyPatch(typeof(ResearchProjectDef), nameof(ResearchProjectDef.CanBeResearchedAt))]
public static class ResearchProjectDef_CanBeResearchedAt_Patches
{
	public static void Postfix(ref bool __result, ResearchProjectDef __instance, Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
	{
		if (!ignoreResearchBenchPowerStatus && PatchesHelper.CanUseNow(bench))
		{
			__result = true;
		}
	}
}

[HarmonyPatch(typeof(Building_WorkTable), nameof(Building_WorkTable.UsableForBillsAfterFueling))]
public static class Building_WorkTable_UsableForBillsAfterFueling_Patches
{
	public static void Postfix(ref bool __result, Building_WorkTable __instance)
	{
		if(PatchesHelper.CanUseNow(__instance))
		{
			__result = true;
		}
	}
}

[HarmonyPatch(typeof(Building_WorkTable), nameof(Building_WorkTable.CurrentlyUsableForBills))]
public static class Building_WorkTable_CurrentlyUsableForBills_Patches
{
	public static void Postfix(ref bool __result, Building_WorkTable __instance)
	{
		if(PatchesHelper.CanUseNow(__instance))
		{
			__result = true;
		}
	}
}
