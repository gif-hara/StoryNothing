using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.AreaControllers.EnterAreaEvents
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

        public UniTask EnterAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            gameController.SetNextArea(nextAreaData);
            return UniTask.CompletedTask;
        }
    }
}
