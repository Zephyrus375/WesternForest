using Microsoft.Xna.Framework;

namespace WesternForest
{
    public class MapArea
    {
        internal Rectangle AreaRect;

        public JsonRectangle Area { get; set; }

        public bool IsOutdoors { get; set; }

        public string MapName { get; set; }

        public Vector2 MapSize { get; set; }

        public Vector2 GetWorldMapPosition(Vector2 localPosition) => !this.IsOutdoors ? new Vector2((float)this.Area.X, (float)this.Area.Y) : new Vector2((float)this.Area.X + localPosition.X / 64f / this.MapSize.X * (float)this.Area.Width, (float)this.Area.Y + localPosition.Y / 64f / this.MapSize.Y * (float)this.Area.Height);
    }
}
