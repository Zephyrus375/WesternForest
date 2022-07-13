using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace WesternForest
{
    internal class WPOTFWorldMap : IClickableMenu
    {
        private static string MapPath = PathUtilities.NormalizeAssetName("LooseSprites/WPOTFWorldMap");
        private static IModHelper Helper;
        private Texture2D image;
        private MapData MapData;
        private WorldMapAreas NPCLocationData;
        private NPCMarker farmerMarker;
        private Rectangle MapRectangle;
        private Vector2 TopLeft;
        private Dictionary<string, Texture2D> NameTexturePairs;
        private const int Radius = 25;

        internal static void Setup(IModHelper helper)
        {
            WPOTFWorldMap.Helper = helper;
        }

        private static void OnKeyPressed(object sender, MenuChangedEventArgs e, Keys key)
        {
            if (key == Keys.P || !(e.NewMenu is GameMenu newMenu) || newMenu.currentTab != 3 || !Game1.currentLocation.Name.StartsWith("Custom_WF"))
                return;
            WPOTFWorldMap.Open((IClickableMenu)newMenu);
        }

        internal static void Open(IClickableMenu gameMenu)
        {
            if (!(gameMenu is GameMenu gameMenu1))
                return;
            Texture2D mapTexture = WPOTFWorldMap.Helper.Content.Load<Texture2D>(WPOTFWorldMap.MapPath, (ContentSource)0);
            Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(mapTexture.Width, mapTexture.Height);
            WPOTFWorldMap menu = new WPOTFWorldMap((int)centeringOnScreen.X, (int)centeringOnScreen.Y, mapTexture);
            gameMenu1.SetChildMenu((IClickableMenu)menu);
        }

        internal WPOTFWorldMap(int x, int y, Texture2D mapTexture)
          : base(x, y, mapTexture.Width, mapTexture.Height, true)
        {
            this.upperRightCloseButton.bounds.X += 18;
            this.upperRightCloseButton.bounds.Y -= 20;
            this.image = mapTexture;
            this.MapData = new MapData("WPOTF/WPOTFWorldMapData");
            this.NPCLocationData = new WorldMapAreas();
            this.TopLeft = Utility.getTopLeftPositionForCenteringOnScreen(this.image.Width, this.image.Height);
            this.MapRectangle = new Rectangle((int)this.TopLeft.X, (int)this.TopLeft.Y, this.image.Width, this.image.Height);
            foreach (NPCMarker npcMarker in this.NPCLocationData.NPCMarkers)
                npcMarker.MapPosition += this.TopLeft - new Vector2(16f, 16f);
            if (this.NPCLocationData.Areas.ContainsKey(Game1.currentLocation.Name))
                this.farmerMarker = new NPCMarker()
                {
                    DisplayName = Game1.player.displayName,
                    MapPosition = this.NPCLocationData.Areas[Game1.currentLocation.Name].GetWorldMapPosition(Game1.player.Position) + this.TopLeft - new Vector2(16f)
                };
            this.NameTexturePairs = new Dictionary<string, Texture2D>();
            foreach (MapLocation mapLocation in this.MapData.Locations.Values)
            {
                if (mapLocation.InhabitantsList != null)
                {
                    foreach (string inhabitants in mapLocation.InhabitantsList)
                    {
                        if (!this.NameTexturePairs.ContainsKey(inhabitants))
                        {
                            if (inhabitants.StartsWith("A:"))
                            {
                                this.NameTexturePairs[inhabitants] = Game1.content.Load<Texture2D>("Animals\\" + inhabitants.Substring(2));
                            }
                            else
                            {
                                NPC characterFromName = Game1.getCharacterFromName(inhabitants);
                                if (characterFromName != null && !characterFromName.isMarried() && characterFromName.CanSocialize)
                                    this.NameTexturePairs[inhabitants] = Game1.content.Load<Texture2D>("Characters\\" + inhabitants);
                            }
                        }
                    }
                }
            }
        }

        public override void draw(SpriteBatch b)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.6f);
            Game1.DrawBox(this.MapRectangle.X, this.MapRectangle.Y, this.MapRectangle.Width, this.MapRectangle.Height);
            b.Draw(this.image, this.TopLeft, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            foreach (NPCMarker npcMarker in this.NPCLocationData.NPCMarkers)
                npcMarker.draw(b);
            if (this.farmerMarker != null)
                Game1.player.FarmerRenderer.drawMiniPortrat(b, this.farmerMarker.MapPosition, 0.5f, 2f, 2, Game1.player);
            bool flag = true;
            Point mousePosition1 = Game1.getMousePosition(true);
            int num1 = mousePosition1.X - 16;
            int num2 = mousePosition1.Y - 16;
            StringBuilder stringBuilder = new StringBuilder();
            List<string> stringList = new List<string>();
            foreach (NPCMarker npcMarker in this.NPCLocationData.NPCMarkers)
            {
                if ((double)Math.Abs(npcMarker.MapPosition.X - (float)num1) < 25.0 && (double)Math.Abs(npcMarker.MapPosition.Y - (float)num2) < 25.0)
                {
                    stringBuilder.Append(npcMarker.DisplayName);
                    stringBuilder.Append(", ");
                    stringList.Add(npcMarker.Name);
                }
            }
            if (stringBuilder.Length > 2)
            {
                flag = false;
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                IClickableMenu.drawHoverText(b, stringBuilder.ToString(), Game1.smallFont, yOffset: -70);
                int x = mousePosition1.X + 40;
                int num3 = mousePosition1.Y + 2 - 70;
                Rectangle rectangle = new Rectangle(0, 0, 16, 20);
                foreach (string key in stringList)
                {
                    Texture2D texture;
                    if (this.NameTexturePairs.TryGetValue(key, out texture) && texture != null)
                    {
                        b.Draw(texture, new Vector2((float)x, (float)(num3 - 30)), new Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
                        x += 50;
                    }
                }
            }
            Point mousePosition2 = Game1.getMousePosition(true);
            int x1 = mousePosition2.X - (int)this.TopLeft.X;
            int y1 = mousePosition2.Y - (int)this.TopLeft.Y;
            foreach (MapLocation mapLocation in this.MapData.Locations.Values)
            {
                if (mapLocation.AreaRect.Contains(x1, y1))
                {
                    IClickableMenu.drawHoverText(b, mapLocation.Text, Game1.smallFont);
                    int x2 = mousePosition2.X + 40;
                    int y2 = mousePosition2.Y + 2;
                    Rectangle rectangle1 = new Rectangle(0, 0, 16, 20);
                    Rectangle rectangle2 = new Rectangle(0, 0, 16, 16);
                    if (mapLocation.InhabitantsList != null)
                    {
                        if (flag)
                        {
                            using (IEnumerator<string> enumerator = mapLocation.InhabitantsList.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    string current = enumerator.Current;
                                    Texture2D texture;
                                    if (this.NameTexturePairs.TryGetValue(current, out texture) && texture != null)
                                    {
                                        if (current.StartsWith("A:"))
                                        {
                                            b.Draw(texture, new Vector2((float)x2, (float)y2), new Rectangle?(rectangle2), Color.White, 0.0f, Vector2.Zero, 2f, SpriteEffects.None, 0.5f);
                                            x2 += 36;
                                        }
                                        else
                                        {
                                            b.Draw(texture, new Vector2((float)x2, (float)(y2 - 30)), new Rectangle?(rectangle1), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
                                            x2 += 50;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
            }
            base.draw(b);
            this.drawMouse(b);
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key == Keys.Escape || key == Keys.M)
                Game1.activeClickableMenu.exitThisMenu();
            base.receiveKeyPress(key);
        }

        public override bool overrideSnappyMenuCursorMovementBan() => true;
    }
}
