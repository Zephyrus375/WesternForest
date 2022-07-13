using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WesternForest
{
    public interface IQuestFrameworkApi
    {
        int ResolveQuestId(string fullQuestName);

        string ResolveQuestName(int questId);

        bool IsManagedQuest(int questId);
    }
}
