using BepInEx;
using BepInEx.Logging;
using BetterAmmoLoadingList.Models;
using BetterAmmoLoadingList.Patch;

namespace BetterAmmoLoadingList
{
    [BepInPlugin("katrin0522.BetterAmmoLoadingList", "Kat.BetterAmmoLoadingList", "1.0.0")]
    public class BetterAmmoLoadingListPlugin : BaseUnityPlugin
    {
        public static BetterAmmoLoadingListPlugin Instance { get; private set; }
        private SettingsModel _settings;
        
        public static ManualLogSource logger;
        private void Awake()
        {
            logger = Logger;
            logger.LogInfo("Plugin loading...");
            
            _settings = SettingsModel.Create(Config);
            
            new CatchMenuLoadAmmoPatch().Enable();
            
            Instance = this;
            
            logger.LogInfo("Plugin successful loaded!");
        }
    }
}
