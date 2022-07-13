using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WesternForest
{
    internal class MapData
    {
        public Dictionary<string, MapLocation> Locations { get; set; }

        public MapData(string dataPath = "WPOTF/WPOTFMapData")
        {
            this.Locations = Game1.content.Load<Dictionary<string, MapLocation>>(PathUtilities.NormalizeAssetName(dataPath));
            foreach (MapLocation mapLocation in this.Locations.Values)
            {
                if (!string.IsNullOrEmpty(mapLocation.Inhabitants))
                    mapLocation.InhabitantsList = ((IEnumerable<string>)mapLocation.Inhabitants.Split('/')).Select<string, string>((Func<string, string>)(name => name.Trim()));
                mapLocation.AreaRect = mapLocation.Area.AsRect();
            }
        }
    }
}
