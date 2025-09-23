using System.Threading;
using Cysharp.Threading.Tasks;

namespace StoryNothing.GameEvents
{
    public class NextAreaAsHome : IGameEvent
    {
        public NextAreaAsHome()
        {
        }

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            gameController.SetNextAreaAsHome();
            return UniTask.CompletedTask;
        }
    }
}
