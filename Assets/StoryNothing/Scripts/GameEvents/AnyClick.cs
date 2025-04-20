using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace StoryNothing.AreaControllers.EnterAreaEvents
{
    public class AnyClick : IGameEvent
    {
        public async UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame, cancellationToken: cancellationToken);
        }
    }
}
