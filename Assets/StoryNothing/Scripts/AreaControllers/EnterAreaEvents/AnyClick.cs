using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace StoryNothing.AreaControllers.EnterAreaEvents
{
    public class AnyClick : IEnterAreaEvent
    {
        public async UniTask EnterAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame, cancellationToken: cancellationToken);
        }
    }
}
