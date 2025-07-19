using System;
using System.Linq;
using System.Reflection;
using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using BetterAmmoLoadingList.Utils;
using SPT.Reflection.Patching;
using UnityEngine;

namespace BetterAmmoLoadingList.Patch
{
    public class CatchMenuLoadAmmoPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Constructor(typeof(GClass3493), new Type[]
            {
                typeof(MagazineItemClass),
                typeof(ItemContextAbstractClass),
                typeof(ItemUiContext)
            });
        }
        
        [PatchPostfix]
        public static void Postfix(GClass3493 __instance, MagazineItemClass magazine, ItemContextAbstractClass itemContext, ItemUiContext uiContext)
        {
            ISession session = PlayerHelper.GetSession();
            MagazineBuildClass magazineBuildClass = session.MagBuildsStorage;
            if (session == null || magazineBuildClass == null)
            {
                return;
            }
            
            __instance.dictionary_0.Clear();
            
            var ammoListRaw = uiContext.FindCompatibleAmmo(magazine).ToList();
            
            
            
            var ammoWithData = ammoListRaw
                .Select(pair => new
                {
                    Pair = pair,
                    Ammo = magazineBuildClass.GetAmmoTemplate(pair.Key)
                })
                .Where(x => x.Ammo != null)
                .ToList();
            
            int maxPen = ammoWithData.Max(x => x.Ammo.PenetrationPower);
            int minPen = ammoWithData.Min(x => x.Ammo.PenetrationPower);
            
            var ammoList = ammoWithData.OrderByDescending(x => x.Ammo.PenetrationPower);
            
            foreach (var entry in ammoList)
            {
                var pair = entry.Pair;

                var ammoClass = new GClass3493.Class2711
                {
                    gclass3493_0 = __instance,
                    ammoType = pair.Key
                };
                
                __instance.bool_0 = true;

                AmmoTemplate ammoData = magazineBuildClass.GetAmmoTemplate(ammoClass.ammoType);

                string valueAmmo = GetPenetrationValue(ammoData, minPen, maxPen);
                
                string text = string.Format(
                    "<b><color=#C6C4B2>{0}</color> <color=#ADB8BC>({1})</color>{2}</b>",
                    LocalizedName(ammoClass.ammoType),
                    pair.Value,
                    valueAmmo
                );

                __instance.method_2(ammoClass.ammoType, text, new Action(ammoClass.method_0), null);
            }
        }

        public static string GetPenetrationValue(AmmoTemplate ammo, int minPen, int maxPen)
        {
            if (ammo?.PenetrationPower != null)
            {
                int pen = ammo?.PenetrationPower ?? 0;
                float t = (maxPen == minPen) ? 1f : (pen - minPen) / (float)(maxPen - minPen);
                t = Mathf.Clamp(t, 0f, 1f);
            
                int r = (int)(255 * (1f - t));
                int g = (int)(255 * t);
                int b = 50;

                string colorHex = $"#{r:X2}{g:X2}{b:X2}";
                return $" <color={colorHex}>[{pen}]</color>";
            }

            return "";
        }
        
        public static string LocalizedName(string itemTemplateId)
        {
            return LocaleManagerClass.LocaleManagerClass.method_4(itemTemplateId + " Name");
        }
    }
}