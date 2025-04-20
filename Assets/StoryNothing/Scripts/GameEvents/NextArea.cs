using System.Threading;
using Cysharp.Threading.Tasks;
using StoryNothing.AreaControllers;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public class NextArea : IGameEvent
    {
        [SerializeField]
        private AreaData nextAreaData;

        public NextArea()
        {
        }

        public NextArea(AreaData nextAreaData)
        {
            this.nextAreaData = nextAreaData;
        }

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            gameController.SetNextArea(nextAreaData);
            return UniTask.CompletedTask;
        }
    }
}
