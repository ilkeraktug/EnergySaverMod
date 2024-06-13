using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace EnergySaverMod.Source.Core.Mod

{
	[StaticConstructorOnStartup]
	public class EnergySaverMod : Verse.Mod
	{
		public EnergySaverMod(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("ilkeraktug.EnergySaver");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			GetSettings<EnerySaverModSettings>();
		}

		public override string SettingsCategory()
		{
			return "Energy Saver";
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			int rowCount = 12;
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect viewRect = new Rect(0f, 0f, inRect.width, rowCount * 26f);
			viewRect.xMax *= 0.9f;
			
			listing_Standard.Begin(viewRect);
			GUI.EndGroup();
			Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

			Widgets.EndScrollView();
		}

		public static Vector2 scrollPosition;
	}
}