using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using System.Collections.Generic;
using xTile;

namespace WesternForest
{
    internal class WorldMapAreas
    {
        private const string path = "assets/WPOTFWorldMapAreas.json";

        public Dictionary<string, MapArea> Areas { get; set; }

        public List<NPCMarker> NPCMarkers { get; set; }

        public WorldMapAreas()
        {
            WPOTFWorldMapAreasModel worldMapAreasModel = ModEntry.Helper.Content.Load<WPOTFWorldMapAreasModel>(PathUtilities.NormalizePath("assets/WPOTFWorldMapAreas.json"), (ContentSource)1);
            MapArea[] areaList = worldMapAreasModel.AreaList;
            this.Areas = new Dictionary<string, MapArea>();
            foreach (MapArea mapArea in areaList)
            {
                GameLocation locationFromName = Game1.getLocationFromName(mapArea.MapName);
                if (locationFromName != null)
                {
                    Map map = locationFromName.Map;
                    mapArea.MapSize = new Vector2((float)map.GetLayer("Back").LayerWidth, (float)map.GetLayer("Back").LayerHeight);
                    mapArea.AreaRect = mapArea.Area.AsRect();
                    this.Areas.Add(mapArea.MapName, mapArea);
                }
            }
            this.NPCMarkers = new List<NPCMarker>();
            foreach (NPC allCharacter in Utility.getAllCharacters())
            {
                MapArea mapArea;
                if (allCharacter.CanSocialize && this.Areas.TryGetValue(allCharacter.currentLocation.Name, out mapArea))
                {
                    int yOffset = 0;
                    if (worldMapAreasModel.DrawYOffsets.ContainsKey(allCharacter.Name))
                        yOffset = worldMapAreasModel.DrawYOffsets[allCharacter.Name];
                    this.NPCMarkers.Add(new NPCMarker(allCharacter, mapArea, yOffset));
                }
            }
        }
    }
}
