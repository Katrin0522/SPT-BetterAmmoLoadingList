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
		public ConfigEntry<bool> GlobalEnable;
		public ConfigEntry<SortOrderType> SortOrder;
		public ConfigEntry<StatAmmoType> StatAmmo;
		
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
						Order = 3
					}));
			
			ColorGradient = configFile.Bind(
				"Settings",
				"ColorGradient",
				true,
				new ConfigDescription("Enable color gradient for context menu ammo load",
					null, 
					new ConfigurationManagerAttributes
					{
						Order = 2
					}));
			
			SortOrder = configFile.Bind(
				"Settings",
				"Sort Order Type",
				SortOrderType.Descending,
				new ConfigDescription("Sort order by penetration power",
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