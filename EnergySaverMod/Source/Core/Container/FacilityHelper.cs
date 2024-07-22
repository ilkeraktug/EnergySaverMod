using System;
using System.Collections.Generic;
using EnergySaverMod.Source.Core.Helper;
using EnergySaverMod.Source.Core.Mod;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Container;

public static class FacilityHelper
{
	public static HashSet<CompFacility> s_FacilityContainer = new();

	public static Dictionary<CompFlickable, bool> s_MultiAnalyzerCheckCache = new();

	public static void TryStore(Building building)
	{
		CompFacility facility = building.GetComp<CompFacility>();
		if (facility != null)
		{
			s_FacilityContainer.Add(facility);
		}
	}

	public static void SetLinkedMultiAnalyzerSwitch(Building_ResearchBench researchBench, bool value)
	{
		if (researchBench == null)
		{
			return;
		}

		CompFacility multiAnalyzerFacilityComp = GetLinkedResearchBench(researchBench);

		if (multiAnalyzerFacilityComp != null)
		{
			if (value == false && AnyOtherLinkedResearchTableRequireMultiAnalyzer(multiAnalyzerFacilityComp, researchBench))
			{
				return;
			}

			if (PatchesHelper.ShouldSwitchPower(multiAnalyzerFacilityComp.parent))
			{
				SetPowerValue(multiAnalyzerFacilityComp.parent, value);
			}
		}
	}

	public static void SetPowerValue(Thing parent, bool value)
	{
		CompFlickable parentFlickable = GetComponentHelper.GetFlickableComponent(parent);

		if (parentFlickable == null)
		{
			return;
		}

		PatchesHelper.SetSwitch(parentFlickable, value);
	}

	public static bool IsMultiAnalyzer(CompFlickable instance)
	{
		if (s_MultiAnalyzerCheckCache.ContainsKey(instance))
		{
			return s_MultiAnalyzerCheckCache[instance];
		}

		bool bHasFacilityComp = instance.parent.GetComp<CompFacility>() != null;

		s_MultiAnalyzerCheckCache.Add(instance, bHasFacilityComp);

		return bHasFacilityComp;
	}

	private static CompFacility GetLinkedResearchBench(Building_ResearchBench researchBench)
	{
		foreach (var facilityComp in s_FacilityContainer)
		{
			foreach (var linkedBuilding in facilityComp.LinkedBuildings)
			{
				if (linkedBuilding == researchBench)
				{
					return facilityComp;
				}
			}
		}

		return null;
	}
	
	public static bool AnyOtherLinkedResearchTableRequireMultiAnalyzer(CompFacility facilityComp,
		Building_ResearchBench researchBench)
	{
		List<Thing> analyzingResearchTables = new();

		if (Find.ResearchManager.GetProject() == null)
		{
			foreach (var pawn in Find.CurrentMap.mapPawns.AllPawns)
			{
				if (pawn.Faction == Find.FactionManager.OfPlayer)
				{
					if (pawn.CurJobDef == JobDefOf.AnalyzeItem && pawn.CurJob != null)
					{
						analyzingResearchTables.Add(pawn.CurJob.targetB.Thing);
					}
				}
			}
		}

		foreach (var linkedBuilding in facilityComp.LinkedBuildings)
		{
			if (linkedBuilding != null && linkedBuilding != researchBench)
			{
				if (Find.ResearchManager.GetProject() == null)
				{
					foreach (var e in analyzingResearchTables)
					{
						if (linkedBuilding == e)
						{
							return true;
						}
					}
				}

				foreach (var pawn in Find.CurrentMap.mapPawns.AllPawns)
				{
					if (pawn.Faction == Find.FactionManager.OfPlayer)
					{
						if (pawn.Position == linkedBuilding.InteractionCell)
						{
							return true;
						}
					}
				}
			}

		}
		
		return false;
	}

	public static bool ShouldModifyPower(Thing __instance)
	{
		if (__instance == null)
		{
			return false;
		}
		
		if(__instance is Building_ResearchBench || 
		    __instance is Building_WorkTable)
		{
			return true;
		}

		return false;
	}
	
	public static bool ShouldModifyPower(CompFlickable flickable)
	{
		return ShouldModifyPower(flickable.parent);
	}
}
		