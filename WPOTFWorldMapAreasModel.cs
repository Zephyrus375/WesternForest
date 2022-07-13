using System.Collections.Generic;

namespace WesternForest
{
    internal class WPOTFWorldMapAreasModel
    {
        public MapArea[] AreaList { get; set; }

        public Dictionary<string, int> DrawYOffsets { get; set; }
    }
}
