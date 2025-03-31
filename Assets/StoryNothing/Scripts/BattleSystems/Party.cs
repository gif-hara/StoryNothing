using System.Collections.Generic;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public sealed class Party
    {
        public List<BattleActor> Actors { get; } = new();

        public void Add(BattleActor actor)
        {
            Actors.Add(actor);
        }
    }
}
