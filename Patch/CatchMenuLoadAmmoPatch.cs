using System;
using System.Linq;
using System.Reflection;
using BetterAmmoLoadingList.Enums;
using BetterAmmoLoadingList.Models;
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
            if(!SettingsModel.Instance.GlobalEnable.Value)
                return;
            
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

            switch (SettingsModel.Instance.StatAmmo.Value)
            {
                case StatAmmoType.PenetrationPower:
                {
                    int maxPen = ammoWithData.Max(x => x.Ammo.PenetrationPower);
                    int minPen = ammoWithData.Min(x => x.Ammo.PenetrationPower);

                    //Standard sort as ascending
                    var ammoList = ammoWithData.OrderBy(x => x.Ammo.PenetrationPower);
            
                    //If we toggle in Descending, do another order
                    if (SettingsModel.Instance.SortOrder.Value == SortOrderType.Descending)
                    {
                        ammoList = ammoWithData.OrderByDescending(x => x.Ammo.PenetrationPower);
                    }
            
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
                
                        string text = $"<b><color=#C6C4B2>{LocalizedName(ammoClass.ammoType)}</color> <color=#ADB8BC>({pair.Value})</color>{valueAmmo}</b>";

                        __instance.method_2(ammoClass.ammoType, text, ammoClass.method_0);
                    }

                    break;
                }
                case StatAmmoType.Damage:
                {
                    int maxDamage = ammoWithData.Max(x => x.Ammo.Damage);
                    int minDamage = ammoWithData.Min(x => x.Ammo.Damage);

                    //Standard sort as ascending
                    var ammoList = ammoWithData.OrderBy(x => x.Ammo.Damage);
            
                    //If we toggle in Descending, do another order
                    if (SettingsModel.Instance.SortOrder.Value == SortOrderType.Descending)
                    {
                        ammoList = ammoWithData.OrderByDescending(x => x.Ammo.Damage);
                    }
            
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

                        string valueAmmo = GetDamageValue(ammoData, minDamage, maxDamage);
                
                        string text = $"<b><color=#C6C4B2>{LocalizedName(ammoClass.ammoType)}</color> <color=#ADB8BC>({pair.Value})</color>{valueAmmo}</b>";

                        __instance.method_2(ammoClass.ammoType, text, ammoClass.method_0);
                    }

                    break;
                }
                case StatAmmoType.VelocitySpeed:
                {
                    int maxSpeed = ammoWithData.Max(x => (int)x.Ammo.InitialSpeed);
                    int minSpeed = ammoWithData.Min(x => (int)x.Ammo.InitialSpeed);

                    //Standard sort as ascending
                    var ammoList = ammoWithData.OrderBy(x => x.Ammo.InitialSpeed);
            
                    //If we toggle in Descending, do another order
                    if (SettingsModel.Instance.SortOrder.Value == SortOrderType.Descending)
                    {
                        ammoList = ammoWithData.OrderByDescending(x => x.Ammo.InitialSpeed);
                    }
            
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

                        string valueAmmo = GetSpeedValue(ammoData, minSpeed, maxSpeed);
                
                        string text = $"<b><color=#C6C4B2>{LocalizedName(ammoClass.ammoType)}</color> <color=#ADB8BC>({pair.Value})</color>{valueAmmo}</b>";

                        __instance.method_2(ammoClass.ammoType, text, ammoClass.method_0);
                    }

                    break;
                }
            }
        }

        private static string GetPenetrationValue(AmmoTemplate ammo, int minPen, int maxPen)
        {
            if (SettingsModel.Instance.ColorGradient.Value)
            {
                if (ammo?.PenetrationPower != null)
                {
                    int penValue = ammo?.PenetrationPower ?? 0;
                    float t = (maxPen == minPen) ? 1f : (penValue - minPen) / (float)(maxPen - minPen);
                    t = Mathf.Clamp(t, 0f, 1f);

                    var colorHex = GetColorGradient(t);
                    return $" <color={colorHex}>[{penValue}]</color>";
                }

                return "";
            }

            return GetPenetrationValue(ammo);
        }
        
        private static string GetDamageValue(AmmoTemplate ammo, int minDamage, int maxDamage)
        {
            if (SettingsModel.Instance.ColorGradient.Value)
            {
                if (ammo?.Damage != null)
                {
                    int damageValue = ammo?.Damage ?? 0;
                    float t = (maxDamage == minDamage) ? 1f : (damageValue - minDamage) / (float)(maxDamage - minDamage);
                    t = Mathf.Clamp(t, 0f, 1f);
            
                    var colorHex = GetColorGradient(t);
                    return $" <color={colorHex}>[{damageValue}]</color>";
                }

                return "";
            }

            return GetDamageValue(ammo);
        }
        
        private static string GetSpeedValue(AmmoTemplate ammo, int minSpeed, int maxSpeed)
        {
            if (SettingsModel.Instance.ColorGradient.Value)
            {
                if (ammo?.InitialSpeed != null)
                {
                    float speedValue = ammo?.InitialSpeed ?? 0;
                    string formattedValue = speedValue + " " + "m/s".Localized();
                    float t = (maxSpeed == minSpeed) ? 1f : (speedValue - minSpeed) / (float)(maxSpeed - minSpeed);
                    t = Mathf.Clamp(t, 0f, 1f);
            
                    var colorHex = GetColorGradient(t);
                    return $" <color={colorHex}>[{formattedValue}]</color>";
                }
            }

            return GetSpeedValue(ammo);
        }

        /// <summary>
        /// Get speed ammo without formatting color
        /// </summary>
        /// <param name="ammo"></param>
        /// <returns></returns>
        private static string GetSpeedValue(AmmoTemplate ammo)
        {
            if (ammo?.InitialSpeed != null)
            {
                string formattedValue = ammo.InitialSpeed + " " + "m/s".Localized();
                    
                return $" [{formattedValue}]";
            }
            return "";
        }
        
        /// <summary>
        /// Get damage ammo without formatting color
        /// </summary>
        /// <param name="ammo"></param>
        /// <returns></returns>
        private static string GetDamageValue(AmmoTemplate ammo)
        {
            if (ammo?.Damage != null)
            {
                return $" [{ammo.Damage}]";
            }

            return "";
        }
        
        /// <summary>
        /// Get penetration power ammo without formatting color
        /// </summary>
        /// <param name="ammo"></param>
        /// <returns></returns>
        private static string GetPenetrationValue(AmmoTemplate ammo)
        {
            if (ammo?.PenetrationPower != null)
            {
                return $" [{ammo.PenetrationPower}]";
            }

            return "";
        }

        private static string LocalizedName(string itemTemplateId)
        {
            return LocaleManagerClass.LocaleManagerClass.method_4(itemTemplateId + " Name");
        }

        private static string GetColorGradient(float t)
        {
            Color colorBest = SettingsModel.Instance.ColorGradientBest.Value;
            Color colorWorst = SettingsModel.Instance.ColorGradientWorst.Value;
                    
            Color finalColor = Color.Lerp(colorWorst, colorBest, t);
            Color32 c32 = finalColor;

            return $"#{c32.r:X2}{c32.g:X2}{c32.b:X2}";
        }
    }
}