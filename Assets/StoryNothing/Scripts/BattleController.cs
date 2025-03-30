using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public class BattleController
    {
        public async UniTask BeginAsync()
        {
            Debug.Log("Battle started!");
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            Debug.Log("Battle ended!");
        }
    }
}
