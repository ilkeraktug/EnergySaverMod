using System.Collections.Generic;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Helper;

public class GetComponentHelper
{
	private static Dictionary<Thing, CompFlickable> CompFlickableMap = new ();
	private static Dictionary<Thing, CompPower> CompPowerMap = new ();
	private static Dictionary<Building, CompPowerTrader> CompPowerTraderMap = new ();
	private static Dictionary<ThingDef, CompProperties_Power> CompPowerPropertiesMap = new ();
	
	public static CompFlickable GetFlickableComponent(Thing thing)
	{
		if (thing == null)
		{
			return null;
		}
		
		if (CompFlickableMap.ContainsKey(thing))
		{
			return CompFlickableMap[thing];
		}

		CompFlickable flickableComponent = thing.TryGetComp<CompFlickable>();

		if (flickableComponent != null)
		{
			CompFlickableMap.Add(thing, flickableComponent);

			return flickableComponent;
		}
		
		return null;
	}
	public static CompPower GetPowerComponent(Thing thing)
	{
		if (thing == null)
		{
			return null;
		}
		
		if (CompPowerMap.ContainsKey(thing))
		{
			return CompPowerMap[thing];
		}

		CompPower powerComponent = thing.TryGetComp<CompPower>();

		if (powerComponent != null)
		{
			CompPowerMap.Add(thing, powerComponent);

			return powerComponent;
		}
		
		return null;
	}
	public static CompPowerTrader GetPowerTraderComponent(Building workTable)
	{
		if (workTable == null)
		{
			return null;
		}
		
		if (CompPowerTraderMap.ContainsKey(workTable))
		{
			return CompPowerTraderMap[workTable];
		}

		CompPowerTrader powerTraderComponent = workTable.TryGetComp<CompPowerTrader>();

		if (powerTraderComponent != null)
		{
			CompPowerTraderMap.Add(workTable, powerTraderComponent);

			return powerTraderComponent;
		}

		return null;
	}
	public static CompProperties_Power GetPowerProperties(ThingDef thingDef)
	{
		if (thingDef == null)
		{
			return null;
		}
		
		if (CompPowerPropertiesMap.ContainsKey(thingDef))
		{
			return CompPowerPropertiesMap[thingDef];
		}

		if (thingDef.CompDefFor<CompPowerTrader>() is CompProperties_Power powerProperties)
		{
			CompPowerPropertiesMap.Add(thingDef, powerProperties);

			return powerProperties;
		}

		return null;
	}
}