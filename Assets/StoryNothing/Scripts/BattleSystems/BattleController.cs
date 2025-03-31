using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public class BattleController
    {
        public Party Players { get; }

        public Party Enemies { get; }

        public BattleController(IBattleSetupData battleSetupData)
        {
            Players = battleSetupData.PlayerParty;
            Enemies = battleSetupData.EnemyParty;
        }

        public async UniTask BeginAsync(HKUIDocument battleDocumentPrefab)
        {
            var battleDocument = UnityEngine.Object.Instantiate(battleDocumentPrefab);
            while (true)
            {
                var actor = GetMovebleActor();
                while (actor == null)
                {
                    foreach (var player in Players.Actors)
                    {
                        player.BehaviourGauge += 0.5f;
                    }
                    foreach (var enemy in Enemies.Actors)
                    {
                        enemy.BehaviourGauge += 0.5f;
                    }
                    await UniTask.NextFrame();
                    actor = GetMovebleActor();
                }
                var (allyParty, opponentParty) = GetParties(actor);
                var targets = await actor.GetTargets(allyParty, opponentParty);
                foreach (var target in targets)
                {
                    target.TakeDamage(actor.CurrentSpec.physicalAttack);
                }
                actor.BehaviourGauge = 0f;
                if (allyParty.IsDeadAll || opponentParty.IsDeadAll)
                {
                    break;
                }
            }
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            battleDocument.DestroySafe();
        }

        public BattleActor GetMovebleActor()
        {
            var actor = Players.GetMovableActor();
            if (actor != null)
            {
                return actor;
            }

            actor = Enemies.GetMovableActor();
            if (actor != null)
            {
                return actor;
            }

            return null;
        }

        public (Party allyParty, Party opponentParty) GetParties(BattleActor actor)
        {
            if (Players.Actors.Contains(actor))
            {
                return (Players, Enemies);
            }
            else if (Enemies.Actors.Contains(actor))
            {
                return (Enemies, Players);
            }
            else
            {
                throw new ArgumentException("Actor is not in any party.");
            }
        }
    }
}
