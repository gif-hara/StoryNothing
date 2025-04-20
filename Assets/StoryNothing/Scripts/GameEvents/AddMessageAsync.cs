using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public class AddMessageAsync : IGameEvent
    {
        [SerializeField]
        private string message = string.Empty;

        public AddMessageAsync()
        {
        }

        public AddMessageAsync(string message)
        {
            this.message = message;
        }

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            gameController.AddMessage(message, cancellationToken);
            return UniTask.CompletedTask;
        }
    }
}
