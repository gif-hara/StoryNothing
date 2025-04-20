using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public sealed class Collection : IGameEvent
    {
        [SerializeField]
        private int collectionId;

        public Collection()
        {
        }

        public Collection(int collectionId)
        {
            this.collectionId = collectionId;
        }

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            if (gameController.CanCollection(collectionId))
            {
                var itemId = gameController.Collection(collectionId);
                gameController.AddMessage($"アイテム {itemId} を手に入れた！");
                return UniTask.CompletedTask;
            }
            else
            {
                gameController.AddMessage("ここには何も無いようだ。");
                return UniTask.CompletedTask;
            }
        }
    }
}
