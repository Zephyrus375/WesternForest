using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;

namespace WesternForest
{
    internal static class SpecialOrders
    {
        private const string FIXBRIDGE = "WPOTF.UntimedSpecialOrder.FixBridge";
        private static IModHelper Helper;
        private static IMonitor Monitor;

        internal static void Initialize(IMod ModInstance)
        {
            SpecialOrders.Helper = ModInstance.Helper;
            SpecialOrders.Monitor = ModInstance.Monitor;
            SpecialOrders.Helper.Events.GameLoop.OneSecondUpdateTicked += new EventHandler<OneSecondUpdateTickedEventArgs>(SpecialOrders.OnUpdate);
        }

        private static void OnUpdate(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (Game1.player.eventsSeen.Contains(85920000) && !Game1.player.team.SpecialOrderActive("WPOTF.UntimedSpecialOrder.FixBridge") && !Game1.player.team.completedSpecialOrders.ContainsKey("WPOTF.UntimedSpecialOrder.FixBridge"))
                Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder("WPOTF.UntimedSpecialOrder.FixBridge", new int?()));
        }
    }
}
