using System;
using System.Collections.Generic;
using EnergySaverMod.Source.Core.Helper;
using EnergySaverMod.Source.Core.Mod;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Container;

public static class FacilityHelper
{
	public static HashSet<CompFacility> s_FacilityContainer = new ();
	
	public static Dictionary<CompFlickable, bool> s_MultiAnalyzerCheckCache = new ();

	public static void TryStore(Building building)
	{
		CompFacility facility = building.GetComp<CompFacility>();
		if(facility != null)
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
			if (PatchesHelper.ShouldSwitchPower(multiAnalyzerFacilityComp.parent))
			{
				SetPowerValue(multiAnalyzerFacilityComp.parent, value);
			}
			else
			{
				Log.Error($"multiAnalyzerFacilityComp ShouldSwitchPower FALSE");
			}
		}
		else
		{
			Log.Error($"multiAnalyzerFacilityComp is NULL");
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
}