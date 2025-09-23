using System.Threading;
using Cysharp.Threading.Tasks;

namespace StoryNothing.GameEvents
{
    public interface IGameEvent
    {
        UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken);
    }
}
