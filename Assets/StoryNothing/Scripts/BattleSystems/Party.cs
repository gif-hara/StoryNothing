using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    [Serializable]
    public sealed class Party
    {
        [SerializeField]
        private List<BattleActor> actors = new();
        public List<BattleActor> Actors => actors;

        public Party()
        {
        }

        public Party(List<BattleActorSpec> actorSpecs)
        {
            foreach (var spec in actorSpecs)
            {
                Add(new BattleActor(spec));
            }
        }

        public void Add(BattleActor actor)
        {
            Actors.Add(actor);
        }
    }
}
