using System;
using System.Collections.Generic;
using StardewValley;

namespace WesternForest
{
    internal class QuestionDialogue
    {
        private List<QuestionAnswer> ltAnswers;
        private string sMessage;
        private List<Response> ltResponses;

        public QuestionDialogue(string sMessage, List<QuestionAnswer> ltAnswers, int iDelay = 100)
        {
            this.ltAnswers = ltAnswers;
            this.sMessage = sMessage;
            this.ltResponses = new List<Response>();
            for (int index = 0; index < ltAnswers.Count; ++index)
                this.ltResponses.Add(new Response(index.ToString(), ltAnswers[index].sKey));
            DelayedAction.functionAfterDelay(new DelayedAction.delayedBehavior(this.DisplayDialogue), iDelay);
        }

        public void DisplayDialogue() => Game1.player.currentLocation.createQuestionDialogue(this.sMessage, this.ltResponses.ToArray(), new GameLocation.afterQuestionBehavior(this.EvaluateAnswer));

        public void EvaluateAnswer(Farmer csFarmer, string sAnswer)
        {
            Action fuHandler = this.ltAnswers[Convert.ToInt32(sAnswer)].fuHandler;
            if (fuHandler == null)
                return;
            fuHandler();
        }
    }
}
