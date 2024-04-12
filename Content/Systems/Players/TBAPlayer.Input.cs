using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria;

namespace TBAC.Content.Systems.Players
{
    public partial class TBAPlayer
    {
        public List<InputCombo> Combos;
        const int TimeOut = 120;
        public int elapsedTime;
        public int usedComboId;
        public bool usedCombo;
        public bool lastLMBState;
        public bool lastRMBState;
        public bool isComboAlive;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TBAInput.GetStand.JustPressed) {
                currentStand = StandLoader.Stands[Main.rand.Next(0, StandLoader.Stands.Count)];
                Main.NewText($"Total Stand Count: {StandLoader.Stands.Count}; Got Stand {currentStand}");
            }

            if (currentStand == -1) // do nothing if we have no stand selected
                return;

            if (Player.controlUseItem && !lastLMBState) {
                ProcessInput(ValidInput.LMB);
            }

            if(Player.controlUseTile && !lastRMBState) {
                ProcessInput(ValidInput.RMB);
            }

            if (TBAInput.SummonStand.JustPressed) {
                isStandActive = !isStandActive;
                isComboAlive = false;
                elapsedTime = 0;
            }

            if(elapsedTime == TimeOut || !isComboAlive) {
                elapsedTime = 0;

                if(isComboAlive) {
                    foreach (var item in Combos) {
                        item.ResetProgress();
                    }
                }

                isComboAlive = false;
            }

            if (elapsedTime < TimeOut && isComboAlive)
                elapsedTime++;

            lastLMBState = Player.controlUseItem;
            lastRMBState = Player.controlUseTile;
        }

        private void ProcessInput(ValidInput input)
        {
            Combos.ForEach(x => x.ProcessInput(input, elapsedTime));
            isComboAlive = true;
            elapsedTime = 0;
        }
    }
}
