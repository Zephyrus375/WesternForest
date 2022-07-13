using StardewModdingAPI;

namespace WesternForest
{
    internal static class Log
    {
        internal static void Error(string msg) => ModEntry.ModMonitor.Log(msg, (LogLevel)4);

        internal static void Alert(string msg) => ModEntry.ModMonitor.Log(msg, (LogLevel)5);

        internal static void Warn(string msg) => ModEntry.ModMonitor.Log(msg, (LogLevel)3);

        internal static void Info(string msg) => ModEntry.ModMonitor.Log(msg, (LogLevel)2);

        internal static void Debug(string msg) => ModEntry.ModMonitor.Log(msg, (LogLevel)1);

        internal static void Trace(string msg) => ModEntry.ModMonitor.Log(msg, (LogLevel)0);
    }
}
