using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    [Serializable]
    public class BattleDebugData : IBattleSetupData
    {
        [SerializeField]
        private List<BattleActorSpec> playerActorSpecs = new();

        [SerializeField]
        private List<BattleActorSpec> enemyActorSpecs = new();

        public Party PlayerParty => new(playerActorSpecs);

        public Party EnemyParty => new(enemyActorSpecs);
    }
}
