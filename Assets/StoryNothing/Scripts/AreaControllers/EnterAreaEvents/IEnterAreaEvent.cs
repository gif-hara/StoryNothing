using System.Threading;
using Cysharp.Threading.Tasks;

namespace StoryNothing.AreaControllers.EnterAreaEvents
{
    public interface IEnterAreaEvent
    {
        UniTask EnterAsync(IGameController gameController, CancellationToken cancellationToken);
    }
}
