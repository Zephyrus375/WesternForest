using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Linq;
using System.Reflection;

namespace WesternForest.HarmonyPatches
{
    internal static class HarmonyPatch_EventDetection
    {
        private static Texture2D WPOTFIcon;

        private static IMonitor Monitor { get; set; }

        private static IModHelper Helper { get; set; }

        private static Rectangle ButtonArea { get; set; }

        private static ClickableComponent WPOTFButton { get; set; }

        internal static void ApplyPatch(Harmony harmony, IModHelper helper)
        {
            HarmonyPatch_EventDetection.Helper = helper;
            Log.Trace("Applying Harmony Patch \"GameMenu_ChangeTab_PostFix.");
            harmony.Patch((MethodBase)AccessTools.Method(typeof(GameMenu), "changeTab", (Type[])null, (Type[])null), new HarmonyMethod(typeof(HarmonyPatch_EventDetection), "GameMenu_ChangeTab_PostFix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
            harmony.Patch((MethodBase)AccessTools.Method(typeof(MapPage), "draw", new Type[1]
            {
        typeof (SpriteBatch)
            }, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(HarmonyPatch_EventDetection), "MapPage_draw_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
            harmony.Patch((MethodBase)AccessTools.Method(typeof(MapPage), "receiveLeftClick", (Type[])null, (Type[])null), new HarmonyMethod(typeof(HarmonyPatch_EventDetection), "MapPage_receiveLeftClick_Prefix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
            harmony.Patch((MethodBase)AccessTools.Constructor(typeof(MapPage), new Type[4]
            {
        typeof (int),
        typeof (int),
        typeof (int),
        typeof (int)
            }, false), (HarmonyMethod)null, new HarmonyMethod(typeof(HarmonyPatch_EventDetection), "MapPage_Constructor_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
            if (HarmonyPatch_EventDetection.Helper.ModRegistry.IsLoaded("Bouhm.NPCMapLocations"))
            {
                try
                {
                    Type type = Type.GetType("NPCMapLocations.Framework.Menus.ModMapPage, NPCMapLocations");
                    harmony.Patch((MethodBase)AccessTools.Method(type, "draw", new Type[1]
                    {
            typeof (SpriteBatch)
                    }, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(HarmonyPatch_EventDetection), "MapPage_draw_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
                }
                catch (Exception ex)
                {
                    Log.Info("Failed patching NPC Map Locations. WPOTF Button will probably be invisible on the map");
                    Log.Info(ex.StackTrace);
                    Log.Info(ex.Message);
                }
            }
            Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(1200, 720);
            HarmonyPatch_EventDetection.ButtonArea = new Rectangle((int)centeringOnScreen.X, (int)centeringOnScreen.Y + 180, 144, 104);
            HarmonyPatch_EventDetection.WPOTFButton = new ClickableComponent(HarmonyPatch_EventDetection.ButtonArea, "")
            {
                myID = 25555,
                rightNeighborID = 1001,
                downNeighborID = 1030,
                upNeighborID = 1001
            };
            HarmonyPatch_EventDetection.Helper.Events.Display.WindowResized += new EventHandler<WindowResizedEventArgs>(HarmonyPatch_EventDetection.OnWindowResized);
        }

        private static void MapPage_Constructor_Postfix(MapPage __instance)
        {
            HarmonyPatch_EventDetection.WPOTFIcon = HarmonyPatch_EventDetection.Helper.Content.Load<Texture2D>(PathUtilities.NormalizeAssetName("LooseSprites/WPOTFIcon"), (ContentSource)0);
            __instance.points.Add(HarmonyPatch_EventDetection.WPOTFButton);
            ClickableComponent clickableComponent1 = __instance.points.Where<ClickableComponent>((Func<ClickableComponent, bool>)(x => x.myID == 1001)).ElementAtOrDefault<ClickableComponent>(0);
            if (clickableComponent1 != null)
            {
                clickableComponent1.leftNeighborID = 25555;
                clickableComponent1.downNeighborID = 25555;
            }
            ClickableComponent clickableComponent2 = __instance.points.Where<ClickableComponent>((Func<ClickableComponent, bool>)(x => x.myID == 1030)).ElementAtOrDefault<ClickableComponent>(0);
            if (clickableComponent2 == null)
                return;
            clickableComponent2.leftNeighborID = 25555;
            clickableComponent2.upNeighborID = 25555;
        }

        private static void OnWindowResized(object sender, WindowResizedEventArgs e)
        {
            Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(1200, 720);
            HarmonyPatch_EventDetection.ButtonArea = new Rectangle((int)centeringOnScreen.X, (int)centeringOnScreen.Y + 496, 397, 288);
            HarmonyPatch_EventDetection.WPOTFButton.bounds = HarmonyPatch_EventDetection.ButtonArea;
        }

        internal static void GameMenu_ChangeTab_PostFix(
          ref GameMenu __instance,
          int whichTab,
          bool playSound = true)
        {
            try
            {
                if (whichTab != 3 || !Game1.currentLocation.Name.StartsWith("Custom_WF"))
                    return;
                WPOTFWorldMap.Open(Game1.activeClickableMenu);
            }
            catch (Exception ex)
            {
                Log.Error("Harmony patch \"GameMenu_ChangeTab_PostFix\" has encountered an error. \n" + ex.ToString());
            }
        }

        internal static void MapPage_draw_Postfix(ref MapPage __instance, SpriteBatch b)
        {
            Game1.drawDialogueBox(HarmonyPatch_EventDetection.ButtonArea.X - 170 + 110, HarmonyPatch_EventDetection.ButtonArea.Y - 16 - 142, 208, 232, false, true);
            b.Draw(HarmonyPatch_EventDetection.WPOTFIcon, new Vector2((float)HarmonyPatch_EventDetection.ButtonArea.X, (float)HarmonyPatch_EventDetection.ButtonArea.Y), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            Point mousePosition = Game1.getMousePosition(true);
            if (HarmonyPatch_EventDetection.ButtonArea.Contains(mousePosition.X, mousePosition.Y))
                IClickableMenu.drawHoverText(b, "WPOTF", Game1.smallFont);
            __instance.drawMouse(b);
        }

        internal static bool MapPage_receiveLeftClick_Prefix(int x, int y, bool playSound)
        {
            if (!HarmonyPatch_EventDetection.ButtonArea.Contains(x, y))
                return true;
            WPOTFWorldMap.Open(Game1.activeClickableMenu);
            return false;
        }
    }
}
