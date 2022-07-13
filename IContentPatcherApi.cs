using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace WesternForest
{
    public interface IContentPatcherApi
    {
        bool IsConditionsApiReady { get; }

        void RegisterToken(IManifest mod, string name, Func<IEnumerable<string>> getValue);

        void RegisterToken(IManifest mod, string name, object token);
    }
}
