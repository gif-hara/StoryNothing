using System.Threading;
using Cysharp.Threading.Tasks;

namespace StoryNothing.AreaControllers.EnterAreaEvents
{
    public interface IGameEvent
    {
        UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken);
    }
}
