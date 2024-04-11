using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAC.Content.Systems
{
    public enum ValidInput
    {
        LMB,
        RMB
    }

    public class InputCombo : IComparable<InputCombo>
    {
        public int progress;
        private int maxProgress;
        private readonly TimedInput[] inputs;
        public Action activationEffect;

        public InputCombo(params TimedInput[] inputs)
        {
            this.inputs = inputs;
            maxProgress = this.inputs.Length;
        }

        public bool ProcessInput(ValidInput input, int elapsedTime)
        {
            var testedInput = inputs[progress];

            if (testedInput.input == input && elapsedTime >= testedInput.elapsedTime && elapsedTime <= testedInput.maxElapsedTime) {
                if(progress <= maxProgress)
                    progress++;

                return true;
            }

            progress = 0;

            return false;
        }

        public bool IsSuccessfull() => progress == maxProgress;

        public void ResetProgress() => progress = 0; // avoids potential "great" idea to manually adjust progress

        public int CompareTo(InputCombo other)
        {
            return -inputs.Length.CompareTo(other.inputs.Length);
        }

        public override string ToString()
        {
            var result = "";
            foreach(var input in inputs) {
                result += $"[{input.input}-{input.elapsedTime}-{input.maxElapsedTime}] [{progress}]";
            }
            return result;
        }
    }

    public struct TimedInput
    {
        public readonly ValidInput input;
        public readonly int elapsedTime;
        public readonly int maxElapsedTime;

        public TimedInput(ValidInput input, int elapsedTime, int maxElapsedTime = 90)
        {
            this.input = input;
            this.elapsedTime = elapsedTime;
            this.maxElapsedTime = maxElapsedTime;
        }
    }
}
