using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WesternForest
{
    public class MapLocation
    {
        internal Rectangle AreaRect;
        internal IEnumerable<string> InhabitantsList;

        public JsonRectangle Area { get; set; }

        public string Inhabitants { get; set; }

        public string Text { get; set; }
    }
}
