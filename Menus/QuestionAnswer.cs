using System;

namespace WesternForest
{
    internal class QuestionAnswer
    {
        public string sKey { get; }

        public Action fuHandler { get; }

        public QuestionAnswer(string sKey, Action fuHandler)
        {
            this.sKey = sKey;
            this.fuHandler = fuHandler;
        }
    }
}
