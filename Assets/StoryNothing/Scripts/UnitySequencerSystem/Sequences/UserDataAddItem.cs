using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using UnityEngine;
using UnitySequencerSystem;

namespace StoryNothing
{
    [Serializable]
    public class UserDataAddItem : ISequence
    {
        [SerializeField]
        private int itemId;

        [SerializeField]
        private int count;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var userData = ServiceLocator.Resolve<UserData>();
            userData.AddItem(itemId, count);
            return UniTask.CompletedTask;
        }
    }
}
