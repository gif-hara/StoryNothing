using System.Threading;
using Cysharp.Threading.Tasks;
using StoryNothing.AreaControllers;
using UnityEngine;
using UnityEngine.Assertions;

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
            Assert.IsNotNull(gameController, "GameController cannot be null.");
            return gameController.AddMessageAsync(message, cancellationToken);
        }
    }
}
