using System.Reflection;
using EnergySaverMod.Source.Core.Container;
using EnergySaverMod.Source.Core.Mod;
using HarmonyLib;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Helper;

public class PatchesHelper
{
	public static bool CanUseNow(Building building, ref bool bShouldCare)
	{
		if (!ShouldSwitchPower(building))
		{
			bShouldCare = false;
			return false;
		}

		if (!FlickableContainer.GetIsAllowed(building) || building.Map.gameConditionManager.ElectricityDisabled(building.Map))
		{
			bShouldCare = true;
			return false;
		}

		var powerTrader = GetComponentHelper.GetPowerTraderComponent(building);
		var powerProperties = GetComponentHelper.GetPowerProperties(building.def);

		float PowerConsumption = 100.0f * CompPower.WattsToWattDaysPerTick;

		if (powerProperties != null)
		{
			PowerConsumption = powerProperties.PowerConsumption * CompPower.WattsToWattDaysPerTick;
		}

		if (powerTrader != null && powerTrader.PowerNet != null)
		{
			if (powerTrader.PowerOn)
			{
				// Log.Message("100");
				bShouldCare = true;
				return true;
			}
			else if (powerTrader.PowerNet.CurrentStoredEnergy() > PowerConsumption ||
			         powerTrader.PowerNet.CurrentEnergyGainRate() > PowerConsumption)
			{
				// Log.Message("200");
				bShouldCare = true;
				return true;
			}
		}

		bShouldCare = false;
		return false;
	}

	public static bool CanWorkWithoutPower(Thing thing)
	{
		CompPower powerComp = GetComponentHelper.GetPowerComponent(thing as Building);

		return powerComp == null || (double)thing.def.building.unpoweredWorkTableWorkSpeedFactor > 0.0;
	}

	public static bool ShouldSwitchPower(Thing thing)
	{
		return FlickableContainer.GetShouldSwitchPower(thing);
	}

	public static void SetSwitch(CompFlickable flickable, bool value)
	{
		if (flickable == null)
		{
			return;
		}
		
		FieldInfo wantSwitchOnFieldInfo = AccessTools.Field(typeof(CompFlickable), "wantSwitchOn");
		wantSwitchOnFieldInfo.SetValue(flickable, value);
				
		flickable.SwitchIsOn = value;

		FlickableContainer.SetPowerValue(flickable, value);
	}
	
}