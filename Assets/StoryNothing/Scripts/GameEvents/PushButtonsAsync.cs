using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public class PushButtonsAsync : IGameEvent
    {
        [SerializeField]
        private List<CreateButtonData> buttonDatabase = new();

        public PushButtonsAsync()
        {
        }

        public PushButtonsAsync(List<CreateButtonData> buttonDatabase)
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
