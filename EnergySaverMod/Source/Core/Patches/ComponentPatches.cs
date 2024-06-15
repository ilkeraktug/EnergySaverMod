using EnergySaverMod.Source.Core.Container;
using HarmonyLib;
using RimWorld;
using Verse;


namespace EnergySaverMod.Source.Core.Patches;

[HarmonyPatch(typeof(CompPowerTrader), "UpdateOverlays")]
	public static class CompPowerTrader_UpdateOverlays_Patches
	{
		public static bool Prefix(CompPowerTrader __instance, ref OverlayHandle? ___overlayPowerOff, ref OverlayHandle? ___overlayNeedsPower)
		{
			if (!FlickableContainer.GetIsAllowed(__instance.parent))
			{
				OverlayHandle? LastHandle = FlickableContainer.GetOverlayHandle(__instance);
				
				__instance.parent.Map.overlayDrawer.Disable((Thing) __instance.parent, ref ___overlayPowerOff);
				__instance.parent.Map.overlayDrawer.Disable((Thing) __instance.parent, ref ___overlayNeedsPower);
				__instance.parent.Map.overlayDrawer.Disable((Thing) __instance.parent, ref LastHandle);
				
				OverlayHandle? handle = __instance.parent.Map.overlayDrawer.Enable(__instance.parent, OverlayTypes.ForbiddenBig);

				FlickableContainer.UpdateOverlayHandle(__instance, handle);
				return false;
			}
			else
			{
				OverlayHandle? handle = FlickableContainer.GetOverlayHandle(__instance);
				__instance.parent.Map.overlayDrawer.Disable(__instance.parent, ref handle);
			}
			
			return true;
		}
	}