using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace WesternForest
{
    internal static class HarmonyPatch_UntimedSO
    {
        private static IMonitor Monitor { get; set; }

        private static IModHelper Helper { get; set; }
        public static void ApplyPatch(Harmony harmony, IModHelper helper)
        {
            HarmonyPatch_UntimedSO.Helper = helper;
            HarmonyPatch_UntimedSO.Helper.Events.GameLoop.DayEnding += new EventHandler<DayEndingEventArgs>(HarmonyPatch_UntimedSO.OnDayEnd);
            Log.Trace("Applying Harmony Patch \"HarmonyPatch_UntimedSO\" prefixing SDV method.");
            harmony.Patch((MethodBase)AccessTools.Method(typeof(SpecialOrder), "IsTimedQuest", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(HarmonyPatch_UntimedSO), "SpecialOrders_IsTimed_postifx", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
            harmony.Patch((MethodBase)AccessTools.Method(typeof(SpecialOrder), "HostHandleQuestEnd", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(HarmonyPatch_UntimedSO), "SpecialOrders_HostHandleQuestEnd_postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
        }

        private static void SpecialOrders_IsTimed_postifx(
          ref SpecialOrder __instance,
          ref bool __result)
        {
            if (!__instance.questKey.Value.StartsWith("WPOTF.UntimedSpecialOrder"))
                return;
            __result = false;
        }
        public static void OnDayEnd(object sender, DayEndingEventArgs e)
        {
            if (!Context.IsMainPlayer)
                return;
            foreach (SpecialOrder specialOrder in Game1.player.team.specialOrders)
            {
                if (specialOrder.questKey.Value.StartsWith("WPOTF.UntimedSpecialOrder"))
                    specialOrder.dueDate.Value = Game1.Date.TotalDays + 100;
            }
        }
    }
}
