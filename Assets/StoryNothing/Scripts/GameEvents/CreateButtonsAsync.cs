using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            gameController.PushButtons(buttonDatabase, cancellationToken);
            return UniTask.CompletedTask;
        }
    }
}
