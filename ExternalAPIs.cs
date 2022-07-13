using StardewModdingAPI;

namespace WesternForest
{
    internal class ExternalAPIs
    {
        public static IJsonAssetsApi JA;
        public static IQuestFrameworkApi QF;

        private static IMonitor Monitor { get; set; }

        private static IModHelper Helper { get; set; }

        internal static void Initialize(IModHelper helper)
        {
            ExternalAPIs.Helper = helper;
            ExternalAPIs.JA = ExternalAPIs.Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (ExternalAPIs.JA == null)
                Log.Warn("Json Assets API not found. This could lead to issues.");
            ExternalAPIs.QF = ExternalAPIs.Helper.ModRegistry.GetApi<IQuestFrameworkApi>("purrplingcat.QuestFramework");
            if (ExternalAPIs.QF != null)
                return;
            Log.Warn("Quest Framework API not found. This could lead to issues.");
        }
    }
}
