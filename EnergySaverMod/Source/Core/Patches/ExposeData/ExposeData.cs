

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
		
		Scribe_Values.Look(ref FlickableContainer.s_FlickableContainer[__instance].Value, "flickableContainer", true, false);
		Scribe_Values.Look(ref FlickableContainer.s_SwitchPowerContainer[__instance].Value, "switchPowerContainer", true, false);
	}
}