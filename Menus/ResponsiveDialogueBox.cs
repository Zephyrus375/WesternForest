using StardewValley;
using StardewValley.Menus;
using System;

namespace WesternForest
{
    internal class ResponsiveDialogueBox : DialogueBox
    {
        private Action AfterDialogueFunction { get; set; }

        public ResponsiveDialogueBox(string sMessage, Action fuFunction)
          : base(sMessage)
        {
            this.AfterDialogueFunction = fuFunction;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.characterIndexInDialogue < this.getCurrentString().Length - 1)
                this.characterIndexInDialogue = this.getCurrentString().Length - 1;
            this.questionFinishPauseTimer = Game1.eventUp ? 600 : 200;
            this.transitioning = true;
            this.transitionInitialized = false;
            this.transitioningBigger = true;
            Game1.dialogueUp = false;
            this.AfterDialogueFunction();
            Game1.objectDialoguePortraitPerson = (NPC)null;
            Game1.playSound("smallSelect");
            this.selectedResponse = -1;
            if (Game1.activeClickableMenu == null || !Game1.activeClickableMenu.Equals((object)this))
                return;
            this.beginOutro();
        }
    }
}
