using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MH3;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public class CreateButtonsAsync : IGameEvent
    {
        [SerializeField]
        private List<CreateButtonData> buttonDatabase = new();

        public CreateButtonsAsync()
        {
        }

        public CreateButtonsAsync(List<CreateButtonData> buttonDatabase)
        {
            this.buttonDatabase = buttonDatabase;
        }

        public async UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            var buttons = gameController.CreateButtons(buttonDatabase.Select(data => data.ButtonText), cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                var (index, behavior) = await buttons.GetBehaviourAsync(cancellationToken);
                var button = buttons[index];
                if (behavior == Define.ButtonBehavior.OnClick)
                {
                    foreach (var i in buttonDatabase[index].OnClickEvents)
                    {
                        await i.Value.InvokeAsync(gameController, cancellationToken);
                    }
                }
                else if (behavior == Define.ButtonBehavior.OnPointerEnter)
                {
                    foreach (var i in buttonDatabase[index].OnPointerEnterEvents)
                    {
                        await i.Value.InvokeAsync(gameController, cancellationToken);
                    }
                }
                else if (behavior == Define.ButtonBehavior.OnPointerExit)
                {
                    foreach (var i in buttonDatabase[index].OnPointerExitEvents)
                    {
                        await i.Value.InvokeAsync(gameController, cancellationToken);
                    }
                }
            }
        }
    }
}
