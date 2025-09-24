using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine.UI;

namespace StoryNothing
{
    public struct HKButton
    {
        public readonly Button Button;

        public HKButton(Button button)
        {
            Button = button;
        }

        public HKButton OnPointerEnter(System.Action<HKButton> action)
        {
            Button.OnPointerEnterAsObservable()
                .Subscribe((this, action), static (_, t) => t.action(t.Item1))
                .RegisterTo(Button.destroyCancellationToken);
            return this;
        }

        public UniTask OnClickAsync(CancellationToken cancellationToken = default)
        {
            return Button.OnClickAsync(cancellationToken);
        }
    }
}
