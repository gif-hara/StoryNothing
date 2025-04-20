using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.GameEvents
{
    public class Log : IGameEvent
    {
        [SerializeField]
        private string message;

        public Log()
        {
        }

        public Log(string logMessage)
        {
            this.message = logMessage;
        }

        public UniTask InvokeAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            Debug.Log(message);
            return UniTask.CompletedTask;
        }
    }
}
