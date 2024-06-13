using System.Collections.Generic;
using EnergySaverMod.Source.Core.Container;
using EnergySaverMod.Source.Core.Helper;
using HarmonyLib;
using RimWorld;
using Verse;

namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(CompFlickable), nameof(CompFlickable.CompGetGizmosExtra))]
public static class CompFlickable_CompGetGizmosExtra_Patches
{
	public static void Postfix(ref IEnumerable<Gizmo> __result, CompFlickable __instance)
	{
		Command_Toggle command_Toggle = new Command_Toggle();
		command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
		command_Toggle.icon = TexCommand.ForbidOff;
		command_Toggle.defaultLabel = "forbidBuildingUsageLabel".Translate();
		command_Toggle.defaultDesc = "forbidBuildingUsageDesc".Translate();
		command_Toggle.isActive = () => FlickableContainer.GetIsActive(__instance);
		command_Toggle.toggleAction = delegate
		{
			FlickableContainer.ToggleIsActive(__instance);
		};

		__result = __result.AddItem(command_Toggle);
	}
}