using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public class BattleActor
    {
        public BattleActorSpec Spec { get; private set; }

        public BattleActor(BattleActorSpec spec)
        {
            Spec = spec;
        }
    }
}
