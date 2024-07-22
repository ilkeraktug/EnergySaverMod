using System;
using System.Collections.Generic;
using EnergySaverMod.Source.Core.Helper;
using EnergySaverMod.Source.Core.Mod;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Container;

public class Bool
{
	public bool Value = true;

	public Bool()
	{
		Value = true;
	}
	public Bool(bool InValue)
	{
		Value = InValue;
	}
}

public static class FlickableContainer
{
	public static Dictionary<CompFlickable, Bool> s_FlickableContainer = new ();
	
	public static Dictionary<CompFlickable, Bool> s_SwitchPowerContainer = new ();
	
	public static Dictionary<CompFlickable, Bool> s_PowerValueContainer = new ();
	
	public static Dictionary<CompPowerTrader, OverlayHandle?> s_OverlayContainer = new ();
	
	public static bool GetIsAllowed(Thing thing)
	{
		//Log.Message($"Thing {thing}");
		CompFlickable flickable = GetComponentHelper.GetFlickableComponent(thing);

		return GetIsAllowed(flickable);
	}
	public static bool GetIsAllowed(CompFlickable flickable)
	{
		if (flickable == null || EnerySaverModSettings.bShouldHideForbidBuilding)
		{
			return true;
		}

		if (s_FlickableContainer.ContainsKey(flickable))
		{
			return s_FlickableContainer[flickable].Value;
		}

		return true;
	}
	public static void ToggleIsAllowed(CompFlickable flickable)
	{
		if (s_FlickableContainer.ContainsKey(flickable))
		{
			s_FlickableContainer[flickable].Value = !s_FlickableContainer[flickable].Value;
			
			return;
		}
		
		Bool value = new Bool(false);
		s_FlickableContainer.Add(flickable, value);
	}
	public static bool GetShouldSwitchPower(Thing thing)
	{
		CompFlickable flickable = GetComponentHelper.GetFlickableComponent(thing);
		
		return GetShouldSwitchPower(flickable);
	}
	public static bool GetShouldSwitchPower(CompFlickable flickable)
	{
		if (flickable == null)
		{
			return false;
		}

		if (s_SwitchPowerContainer.ContainsKey(flickable))
		{
			return s_SwitchPowerContainer[flickable].Value;
		}

		return !PatchesHelper.CanWorkWithoutPower(flickable.parent);
	}

	public static void SetPowerValue(Thing thing, bool value)
	{
		CompFlickable flickable = GetComponentHelper.GetFlickableComponent(thing);
		
		SetPowerValue(flickable, value);
	}
	public static void SetPowerValue(CompFlickable flickable, bool value)
	{
		if (flickable == null)
		{
			return;
		}
		
		Bool myValue = new Bool(value);
		
		if (s_PowerValueContainer.ContainsKey(flickable))
		{
			s_PowerValueContainer[flickable] = myValue;
		}
		else
		{
			s_PowerValueContainer.Add(flickable, myValue);
		}
	}
	public static bool GetPowerValue(Thing thing)
	{
		CompFlickable flickable = GetComponentHelper.GetFlickableComponent(thing);
		
		return GetPowerValue(flickable);
	}
	public static bool GetPowerValue(CompFlickable flickable)
	{
		if (flickable == null)
		{
			return true;
		}

		if (s_PowerValueContainer.ContainsKey(flickable))
		{
			return s_PowerValueContainer[flickable].Value;
		}
		
		return false;
	}
	
	public static void ToggleShouldSwitchPower(CompFlickable flickable)
	{
		if (s_SwitchPowerContainer.ContainsKey(flickable))
		{
			s_SwitchPowerContainer[flickable].Value = !s_SwitchPowerContainer[flickable].Value;
			
			return;
		}

		Bool value;
		if (!PatchesHelper.CanWorkWithoutPower(flickable.parent))
		{
			value = new Bool(false);
		}
		else
		{
			value = new Bool(true);
		}
		
		s_SwitchPowerContainer.Add(flickable, value);
	}

	public static OverlayHandle? GetOverlayHandle(CompPowerTrader powerTrader)
	{
		if (s_OverlayContainer.ContainsKey(powerTrader))
		{
			return s_OverlayContainer[powerTrader];
		}
		
		return new OverlayHandle();
	}
	
	public static void UpdateOverlayHandle(CompPowerTrader powerTrader, OverlayHandle? handle)
	{
		if (s_OverlayContainer.ContainsKey(powerTrader))
		{
			s_OverlayContainer[powerTrader] = handle;
			return;
		}

		s_OverlayContainer.Add(powerTrader, handle);
	}
}