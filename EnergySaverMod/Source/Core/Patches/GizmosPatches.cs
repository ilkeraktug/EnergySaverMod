using System.Collections.Generic;
using System.Linq;
using EnergySaverMod.Source.Core.Container;
using EnergySaverMod.Source.Core.Helper;
using EnergySaverMod.Source.Core.Mod;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(CompFlickable), nameof(CompFlickable.CompGetGizmosExtra))]
public static class CompFlickable_CompGetGizmosExtra_Patches
{
	public static void Postfix(ref IEnumerable<Gizmo> __result, CompFlickable __instance)
	{
		if (!(__instance.parent is Building_WorkTable || __instance.parent is Building_ResearchBench || FacilityHelper.IsMultiAnalyzer(__instance)))
		{
			return;
		}

		if (EnerySaverModSettings.bShouldHideTogglePower && !PatchesHelper.CanWorkWithoutPower(__instance.parent))
		{
			RemoveToggleElectricGizmo(ref __result);
		}

		{
			bool bShouldSwitchPower = FlickableContainer.GetShouldSwitchPower(__instance);
			Command_Toggle command_Switch = new Command_Toggle();
			command_Switch.hotKey = KeyBindingDefOf.Misc4;
			command_Switch.icon = bShouldSwitchPower ? TextureContainer.SwitchTexGreen : TextureContainer.SwitchTexRed;
			command_Switch.defaultLabel = "shouldSwitchPowerLabel".Translate();
			command_Switch.defaultDesc = "shouldSwitchPowerDesc".Translate();
			command_Switch.isActive = () => bShouldSwitchPower;
			command_Switch.toggleAction = delegate
			{
				FlickableContainer.ToggleShouldSwitchPower(__instance);
			};
			
			__result = __result.Append(command_Switch);
		}

		if (!EnerySaverModSettings.bShouldHideForbidBuilding)
		{
			Command_Toggle command_ToggleAllow = new Command_Toggle();
			command_ToggleAllow.hotKey = KeyBindingDefOf.Command_ItemForbid;
			command_ToggleAllow.icon = TexCommand.ForbidOff;
			command_ToggleAllow.defaultLabel = "allowUseBuildingUsageLabel".Translate();
			command_ToggleAllow.defaultDesc = "allowUseBuildingUsageDesc".Translate();
			command_ToggleAllow.isActive = () => FlickableContainer.GetIsAllowed(__instance);
			command_ToggleAllow.toggleAction = delegate
			{
				FlickableContainer.ToggleIsAllowed(__instance);

				if (!FlickableContainer.GetIsAllowed(__instance))
				{
					// __instance.SwitchIsOn = false;
					// __instance.WantsFlick();
					
					PatchesHelper.SetSwitch(__instance, false);
				}

				var powerTraderComponent = GetComponentHelper.GetPowerTraderComponent(__instance.parent as Building);
				var method = AccessTools.Method(typeof(CompPowerTrader), "UpdateOverlays");
				if (powerTraderComponent != null && method != null)
				{
					method.Invoke(powerTraderComponent, new object[] { });
				}
			};

			__result = __result.Append(command_ToggleAllow);
		}
	}

	private static void RemoveToggleElectricGizmo(ref IEnumerable<Gizmo> __result)
	{
		var resultAsList = __result.ToList();

		foreach (Gizmo gizmo in resultAsList)
		{
			if (gizmo is Command command)
			{
				if (command.defaultDesc == "CommandDesignateTogglePowerDesc".Translate().ToString())
				{
					resultAsList.Remove(gizmo);
					break;
				}
			}
		}

		__result = resultAsList.AsEnumerable();
	}
}