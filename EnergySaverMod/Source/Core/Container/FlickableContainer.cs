using System.Collections.Generic;
using EnergySaverMod.Source.Core.Helper;
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

	public static bool GetIsActive(Thing thing)
	{
		CompFlickable flickable = GetComponentHelper.GetFlickableComponent(thing);
		
		if (flickable == null)
		{
			return false;
		}

		if (s_FlickableContainer.ContainsKey(flickable))
		{
			return s_FlickableContainer[flickable].Value;
		}

		return true;
	}
	public static bool GetIsActive(CompFlickable flickable)
	{
		if (flickable == null)
		{
			return false;
		}

		if (s_FlickableContainer.ContainsKey(flickable))
		{
			return s_FlickableContainer[flickable].Value;
		}

		return true;
	}
	public static void ToggleIsActive(CompFlickable flickable)
	{
		if (s_FlickableContainer.ContainsKey(flickable))
		{
			s_FlickableContainer[flickable].Value = !s_FlickableContainer[flickable].Value;
			
			return;
		}
		
		Bool value = new Bool(false);
		s_FlickableContainer.Add(flickable, value);
	}
}