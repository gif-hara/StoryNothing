using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    [Serializable]
    public class BattleDebugData
    {
        [SerializeField]
        private List<BattleActorSpec> players = new();
        public List<BattleActorSpec> Players => players;

        [SerializeField]
        private List<BattleActorSpec> enemies = new();
        public List<BattleActorSpec> Enemies => enemies;
    }
}
