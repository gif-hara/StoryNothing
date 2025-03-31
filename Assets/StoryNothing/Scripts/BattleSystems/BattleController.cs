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
        public Party Players { get; } = new Party();

        public Party Enemies { get; } = new Party();

        public async UniTask BeginAsync(HKUIDocument battleDocumentPrefab)
        {
            var battleDocument = UnityEngine.Object.Instantiate(battleDocumentPrefab);
            Players.Add(new BattleActor("Player1", 100));
            Players.Add(new BattleActor("Player2", 100));
            Enemies.Add(new BattleActor("Enemy1", 100));
            Enemies.Add(new BattleActor("Enemy2", 100));
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            battleDocument.DestroySafe();
        }
    }
}
