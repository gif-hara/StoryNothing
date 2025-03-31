using System;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public class BattleController
    {
        public async UniTask BeginAsync(HKUIDocument battleDocumentPrefab)
        {
            var battleDocument = UnityEngine.Object.Instantiate(battleDocumentPrefab);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            battleDocument.DestroySafe();
        }
    }
}
