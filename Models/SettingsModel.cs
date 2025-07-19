using BepInEx.Configuration;
using BetterAmmoLoadingList.Enums;
using BetterAmmoLoadingList.Utils;
using UnityEngine;

namespace BetterAmmoLoadingList.Models
{
	/// <summary>
	/// Model with config fields
	/// </summary>
	public class SettingsModel
	{
		public static SettingsModel Instance { get; private set; }

		public ConfigEntry<bool> ColorGradient;
		public ConfigEntry<SortOrderType> SortOrder;
		public ConfigEntry<StatAmmoType> StatAmmo;
		
#if DEBUG
		public ConfigEntry<KeyboardShortcut> SavePos;
		public ConfigEntry<KeyboardShortcut> SeeDistance;
		public ConfigEntry<float> PositionXDebug;
		public ConfigEntry<float> PositionYDebug;
		public ConfigEntry<int> FontSizeDebug;
#endif
		
		private SettingsModel(ConfigFile configFile)
		{
			ColorGradient = configFile.Bind(
				"Settings",
				"ColorGradient",
				true,
				new ConfigDescription("Enable color gradient for context menu ammo load"));
			
			SortOrder = configFile.Bind(
				"Settings",
				"Sort Order Type",
				SortOrderType.Descending,
				new ConfigDescription("Sort order by penetration power"));
			
			StatAmmo = configFile.Bind(
				"Settings",
				"Sort ammo by stat type",
				StatAmmoType.PenetrationPower,
				new ConfigDescription("Sort order by selected stat ammo type"));
			
#if DEBUG
			#region Overlay

			PositionXDebug = configFile.Bind(
				"Settings",
				"PositionX",
				10f,
				new ConfigDescription("X Position", new AcceptableValueRange<float>(-2000f, 2000f)));

			PositionYDebug = configFile.Bind(
				"Settings",
				"PositionY",
				-10f,
				new ConfigDescription("Y Position", new AcceptableValueRange<float>(-2000f, 2000f)));
			
			FontSizeDebug = configFile.Bind(
				"Settings",
				"FontSizeDebug",
				28,
				new ConfigDescription("FontSizeDebug", new AcceptableValueRange<int>(0, 200)));

			#endregion

			PositionXDebug.SettingChanged += (_, __) =>
			{
				OverlayDebug.Instance.SetOverlayPosition(new Vector2(PositionXDebug.Value, PositionYDebug.Value));
			};
			
			PositionYDebug.SettingChanged += (_, __) =>
			{
				OverlayDebug.Instance.SetOverlayPosition(new Vector2(PositionXDebug.Value, PositionYDebug.Value));
			};

			FontSizeDebug.SettingChanged += (_, __) =>
			{
				OverlayDebug.Instance.SetFontSize(FontSizeDebug.Value);
			};
#endif
		}
		
		/// <summary>
		/// Init configs model
		/// </summary>
		/// <param name="configFile"></param>
		/// <returns></returns>
		public static SettingsModel Create(ConfigFile configFile)
		{
			if (Instance != null)
			{
				return Instance;
			}
			return Instance = new SettingsModel(configFile);
		}
	}
}