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
		public ConfigEntry<Color> ColorGradientBest;
		public ConfigEntry<Color> ColorGradientWorst;
		public ConfigEntry<bool> GlobalEnable;
		public ConfigEntry<SortOrderType> SortOrder;
		public ConfigEntry<StatAmmoType> StatAmmo;
		public ConfigEntry<bool> ShowAllStats;
		public ConfigEntry<bool> IsWrapText;
		
		private SettingsModel(ConfigFile configFile)
		{
			GlobalEnable = configFile.Bind(
				"Settings",
				"Enable feature",
				true,
				new ConfigDescription("Enable sort/coloring/show stats ammo in load list",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 5
					}));
			
			ColorGradient = configFile.Bind(
				"Settings",
				"Color Gradient",
				true,
				new ConfigDescription("Enable color gradient for context menu ammo load",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 4
					}));
			
			ColorGradientBest = configFile.Bind(
				"Settings",
				"Color Gradient Best",
				Color.green,
				new ConfigDescription("Color preset for best ammo",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 3
					}));
			
			ColorGradientWorst = configFile.Bind(
				"Settings",
				"Color Gradient Worst",
				Color.red,
				new ConfigDescription("Color preset for worst ammo",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 2
					}));
			
			SortOrder = configFile.Bind(
				"Settings",
				"Sort Order Type",
				SortOrderType.Descending,
				new ConfigDescription("Sort order by selected type",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 1
					}));
			
			StatAmmo = configFile.Bind(
				"Settings",
				"Sort ammo by stat type",
				StatAmmoType.PenetrationPower,
				new ConfigDescription("Sort order by selected stat ammo type",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 0
					}));
			
			ShowAllStats = configFile.Bind(
				"[EXPERIMENTAL SETTINGS]",
				"Show All Stats",
				false,
				new ConfigDescription("[EXPERIMENTAL] Show all stats in list, them will be sorted by selected stat type, selected type will be colored, other non color",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 1
					}));
			
			IsWrapText = configFile.Bind(
				"[EXPERIMENTAL SETTINGS]",
				"Warp Text Stats",
				false,
				new ConfigDescription("[EXPERIMENTAL] When this setting is enabled, the ammunition stats will wrap to the following line",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 0
					}));

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