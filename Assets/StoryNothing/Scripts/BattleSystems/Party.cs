using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    [Serializable]
    public sealed class Party
    {
        [SerializeField]
        private List<BattleActor> actors = new();
        public List<BattleActor> Actors => actors;

        public bool IsDeadAll => actors.All(actor => actor.IsDead);

        [CanBeNull]
        public BattleActor GetMovableActor()
        {
            return actors.Find(actor => actor.BehaviourGauge >= 1f && !actor.IsDead);
        }

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
