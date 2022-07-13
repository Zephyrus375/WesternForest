using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace WesternForest
{
    public class NPCMarker
    {
        public Vector2 MapPosition;
        public string DisplayName;
        public string Name;
        public Texture2D Texture;
        public Rectangle SourceRectangle;

        public NPCMarker()
        {
        }

        public NPCMarker(NPC npc, MapArea mapArea, int yOffset = 0)
        {
            this.Name = npc.Name;
            this.DisplayName = npc.displayName;
            this.Texture = Game1.content.Load<Texture2D>(PathUtilities.NormalizeAssetName("Characters//" + npc.Name));
            this.MapPosition = mapArea.GetWorldMapPosition(npc.Position);
            this.SourceRectangle = new Rectangle(0, yOffset, 16, 16);
        }

        public void draw(SpriteBatch b) => b.Draw(this.Texture, this.MapPosition, new Rectangle?(this.SourceRectangle), Color.White, 0.0f, Vector2.Zero, 2f, SpriteEffects.None, 0.5f);
    }
}
