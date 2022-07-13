using HarmonyLib;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace WesternForest
{
    internal class TileAction
    {
        internal string Key;
        internal Action Method;
        internal MethodInfo MethodAsInfo;
        internal static List<TileAction> Actions = new List<TileAction>();
        internal static readonly Harmony Harmony = ModEntry.Instance.harmony;

        internal TileAction(string key, Action action)
        {
            this.Key = key;
            this.Method = action;
            TileAction.Actions.Add(this);
        }

        internal TileAction(string key, MethodInfo method)
        {
            this.Key = key;
            this.MethodAsInfo = method;
            TileAction.Actions.Add(this);
        }

        internal static void Patch() => TileAction.Harmony.Patch((MethodBase)AccessTools.Method(typeof(GameLocation), "performAction", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(TileAction), "Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);

        internal static void Postfix(ref string action, ref bool __result)
        {
            foreach (TileAction action1 in TileAction.Actions)
            {
                if (action1.Key == action.Split()[0])
                {
                    if (action.Split(' ').Length == 1)
                        action1.Method();
                    else
                        action1.MethodAsInfo.Invoke((object)null, new object[1]
                        {
              (object) action.Split(' ')
                        });
                    __result = true;
                    return;
                }
            }
            __result = false;
        }
    }
}
