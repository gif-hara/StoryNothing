using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public sealed class SetupCollectionSpot : IGameEvent
    {
        [SerializeField]
        private List<CollectionSpotBlueprint> collectionSpots = new();

        public SetupCollectionSpot()
        {
        }

        public SetupCollectionSpot(List<CollectionSpotBlueprint> collectionSpots)
        {
            this.collectionSpots = collectionSpots;
        }

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            gameController.SetupCollectionSpot(collectionSpots.Select(x => x.Create()).ToList());
            return UniTask.CompletedTask;
        }
    }
}
