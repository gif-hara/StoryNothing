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
                var events = behavior switch
                {
                    Define.ButtonBehavior.OnClick => buttonDatabase[index].OnClickEvents,
                    Define.ButtonBehavior.OnPointerEnter => buttonDatabase[index].OnPointerEnterEvents,
                    Define.ButtonBehavior.OnPointerExit => buttonDatabase[index].OnPointerExitEvents,
                    _ => throw new System.Exception("Invalid behavior.")
                };
                foreach (var i in events)
                {
                    await i.Value.InvokeAsync(gameController, cancellationToken);
                }
            }
        }
    }
}
