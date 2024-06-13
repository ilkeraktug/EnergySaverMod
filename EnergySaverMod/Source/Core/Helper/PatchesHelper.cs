using EnergySaverMod.Source.Core.Container;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Helper;

public class PatchesHelper
{
	public static bool CanUseNow(Building building)
	{
		var powerTrader = GetComponentHelper.GetPowerTraderComponent(building);
		var powerProperties = GetComponentHelper.GetPowerProperties(building.def);

		float PowerConsumption = 100.0f;

		if (powerProperties != null)
		{
			PowerConsumption = powerProperties.PowerConsumption;
		}
		
		if (powerTrader != null && powerTrader.PowerNet != null)
		{
			return FlickableContainer.GetIsActive(building) && 
			       powerTrader.PowerOn == false &&
			       (powerTrader.PowerNet.CurrentStoredEnergy() > PowerConsumption ||
			        powerTrader.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick > PowerConsumption);
		}

		return false;
	}

	public static bool CanWorkWithoutPower(Thing thing)
	{
		CompPower powerComp = GetComponentHelper.GetPowerComponent(thing as Building);
		
		return powerComp == null || (double) thing.def.building.unpoweredWorkTableWorkSpeedFactor > 0.0;
	}
}