using Verse;

namespace EnergySaverMod.Source.Core.Mod;

public class EnerySaverModSettings : ModSettings
{
	public static bool bShouldSpawnUnpowered = true;
	
	public static bool bShouldHideTogglePower = true;
	public static bool bShouldHideForbidBuilding = false;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref bShouldSpawnUnpowered, "bShouldSpawnUnpowered", true, false);
		
		Scribe_Values.Look(ref bShouldHideTogglePower, "bShouldHideTogglePower", true, false);
		Scribe_Values.Look(ref bShouldHideForbidBuilding, "bShouldHideForbidBuilding", false, false);
	}
}