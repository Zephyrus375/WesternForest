using Microsoft.Xna.Framework;

namespace WesternForest
{
    public struct JsonRectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public JsonRectangle(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Rectangle AsRect() => new Rectangle(this.X, this.Y, this.Width, this.Height);
    }
}
