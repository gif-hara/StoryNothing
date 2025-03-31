using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public class BattleActor
    {
        public BattleActorSpec BaseSpec { get; private set; }

        public BattleActorSpec CurrentSpec { get; }

        public bool IsDead => CurrentSpec.hitPoint <= 0;

        public float BehaviourGauge { get; set; }

        public BattleActor(BattleActorSpec spec)
        {
            BaseSpec = spec;
            CurrentSpec = spec.Clone();
            BehaviourGauge = 0f;
        }

        public UniTask<List<BattleActor>> GetTargets(Party allyParty, Party opponentParty)
        {
            var result = new List<BattleActor>();
            var index = Random.Range(0, opponentParty.Actors.Count);
            var target = opponentParty.Actors[index];
            result.Add(target);
            return UniTask.FromResult(result);
        }

        public void TakeDamage(int damage)
        {
            CurrentSpec.hitPoint -= damage;
            if (CurrentSpec.hitPoint < 0)
            {
                CurrentSpec.hitPoint = 0;
            }
            Debug.Log($"{BaseSpec.name} took {damage} damage. Remaining HP: {CurrentSpec.hitPoint}");
        }
    }
}
