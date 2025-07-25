﻿using Comfort.Common;
using EFT;
using EFT.UI;
using SPT.Reflection.Utils;

namespace BetterAmmoLoadingList.Utils
{
    public class PlayerHelper
    {
        private static PlayerHelper _instance;

        public static PlayerHelper Instance => _instance ??= new PlayerHelper();
    
        public static ISession GetSession(bool throwIfNull = false)
        {
            var session = ClientAppUtils.GetClientApp().Session;

            if (throwIfNull && session is null)
            {
                BetterAmmoLoadingListPlugin.logger.LogWarning("Trying to access the Session when it's null");
            }

            return session;
        }
    
        public static Profile GetProfile(bool throwIfNull = false)
        {
            var profile = GetSession()?.Profile;

            if (throwIfNull && profile is null)
            {
                BetterAmmoLoadingListPlugin.logger.LogWarning("Trying to access the Profile when it's null");
            }
        
            return GetSession()?.Profile;
        }
    
        public static bool HasRaidStarted()
        {
            bool? inRaid = Singleton<AbstractGame>.Instance?.InRaid;
            return inRaid.HasValue && inRaid.Value;
        }
    
        public Player Player { get; set; }
    
        public EftBattleUIScreen EftBattleUIScreen { get; set; }
    }
}