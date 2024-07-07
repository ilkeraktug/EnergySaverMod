using System;
using EnergySaverMod.Source.Core.Helper;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;


namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(StudyUtility), "TryFindResearchBench")]
public static class StudyUtility_TryFindResearchBench_Patches
{
	public static bool Prefix(ref bool __result, Pawn pawn, out Building_ResearchBench bench)
	{
		bench = (Building_ResearchBench) GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Some), validator: ((Predicate<Thing>) (t =>
		{
			if (!pawn.CanReserve((LocalTargetInfo) t))
				return false;

			Building_ResearchBench localBench = t as Building_ResearchBench;
			CompPowerTrader powerTrader = GetComponentHelper.GetPowerTraderComponent(localBench);
			
			bool bShouldCare = false;
			bool result = PatchesHelper.CanUseNow(localBench, ref bShouldCare);

			if (bShouldCare)
			{
				return result;
			}

			return powerTrader == null || powerTrader.PowerOn;
		})));
		
		__result = bench != null;

		return false;
	}
}