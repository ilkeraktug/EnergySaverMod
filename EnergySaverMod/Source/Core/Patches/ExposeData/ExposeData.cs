

using EnergySaverMod.Source.Core.Container;
using HarmonyLib;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Patches.ExposeData;

[HarmonyPatch(typeof(CompFlickable), nameof(CompFlickable.PostExposeData), null)]
public static class CompFlickable_ExposeData_Patcher
{
	public static void Postfix(CompFlickable __instance)
	{
		if (!FlickableContainer.s_FlickableContainer.ContainsKey(__instance))
		{
			FlickableContainer.s_FlickableContainer.Add(__instance, new Bool());
		}
		
		if (!FlickableContainer.s_SwitchPowerContainer.ContainsKey(__instance))
		{
			FlickableContainer.s_SwitchPowerContainer.Add(__instance, new Bool());
		}

		bool value = true;
		if (!FlickableContainer.s_PowerValueContainer.ContainsKey(__instance))
		{
			if (FacilityHelper.ShouldModifyPower(__instance))
			{
				value = false;
			}
			
			FlickableContainer.s_PowerValueContainer.Add(__instance, new Bool(value));
		}
		else
		{
			FlickableContainer.s_PowerValueContainer[__instance].Value = __instance.SwitchIsOn;
		}

		Scribe_Values.Look(ref FlickableContainer.s_FlickableContainer[__instance].Value, "flickableContainer", true, false);
		Scribe_Values.Look(ref FlickableContainer.s_SwitchPowerContainer[__instance].Value, "switchPowerContainer", true, false);
		Scribe_Values.Look(ref FlickableContainer.s_PowerValueContainer[__instance].Value, "IsPowerOffContainer", false, false);
	}
}