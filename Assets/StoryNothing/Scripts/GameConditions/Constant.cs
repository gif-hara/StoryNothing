using UnityEngine;

namespace StoryNothing.GameConditions
{
    public sealed class Constant : IGameCondition
    {
        [SerializeField]
        private bool value = true;

        public Constant()
        {
        }

        public Constant(bool value)
        {
            this.value = value;
        }

        public bool Evaluate(IGameController gameController)
        {
            return value;
        }
    }
}
