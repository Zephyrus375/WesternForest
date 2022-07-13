using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WesternForest
{
    public class ModEntry : Mod
    {
        private List<QuestionAnswer> ltAnswers = new List<QuestionAnswer>();
        private GameLocation newLoc;
        private GameLocation oldLoc;
        private Farmer who;

        internal static ModEntry Instance { get; private set; }

        internal Harmony harmony { get; private set; }

        internal static IMonitor ModMonitor { get; set; }

        internal static IModHelper Helper { get; set; }

        public override void Entry(IModHelper helper)
        {
            ModEntry.ModMonitor = this.Monitor;
            ModEntry.Helper = helper;
            if (!ModEntry.Helper.ModRegistry.IsLoaded("Zephyrus.WesternForest.CP"))
            {
                Log.Error("Western Forest appears to be installed incorrectly. Delete it and reinstall it please.");
            }
            else
            {
                SpecialOrders.Initialize((IMod)this);
            }

        }
        private void OnGameLaunched(object sender, EventArgs e)
        {
            WPOTFWorldMap.Setup(ModEntry.Helper);
            ExternalAPIs.Initialize(ModEntry.Helper);
        }
    }
}
