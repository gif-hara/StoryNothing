using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using StoryNothing;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MH3
{
    public static partial class Extensions
    {
        public static UniTask<PointerEventData> OnPointerEnterAsync(this Button button, CancellationToken cancellationToken)
        {
            var source = new UniTaskCompletionSource<PointerEventData>();
            button.OnPointerEnterAsObservable()
                .Subscribe(source, static (eventData, source) =>
                {
                    source.TrySetResult(eventData);
                })
                .RegisterTo(cancellationToken);
            return source.Task;
        }

        public static UniTask<PointerEventData> OnPointerExitAsync(this Button button, CancellationToken cancellationToken)
        {
            var source = new UniTaskCompletionSource<PointerEventData>();
            button.OnPointerExitAsObservable()
                .Subscribe(source, static (eventData, source) =>
                {
                    source.TrySetResult(eventData);
                })
                .RegisterTo(cancellationToken);
            return source.Task;
        }

        public static async UniTask<(int index, Define.ButtonBehavior behavior)> GetBehaviourAsync(this IEnumerable<Button> buttons, CancellationToken cancellationToken)
        {
            var onClickResults = UniTask.WhenAny(buttons.Select(button => button.OnClickAsync(cancellationToken)));
            var onPointerEnterResults = UniTask.WhenAny(buttons.Select(button => button.OnPointerEnterAsync(cancellationToken)));
            var onPointerExitResults = UniTask.WhenAny(buttons.Select(button => button.OnPointerExitAsync(cancellationToken)));
            var (winArgumentIndex, result1, result2, result3) = await UniTask.WhenAny(onClickResults, onPointerEnterResults, onPointerExitResults);
            switch (winArgumentIndex)
            {
                case 0:
                    return (result1, Define.ButtonBehavior.OnClick);
                case 1:
                    return (result2.winArgumentIndex, Define.ButtonBehavior.OnPointerEnter);
                case 2:
                    return (result3.winArgumentIndex, Define.ButtonBehavior.OnPointerExit);
                default:
                    throw new System.Exception("Invalid result index.");
            }
        }
    }
}
