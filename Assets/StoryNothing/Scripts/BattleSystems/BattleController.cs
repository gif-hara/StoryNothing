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
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            battleDocument.DestroySafe();
        }
    }
}
