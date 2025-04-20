using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.AreaControllers.EnterAreaEvents
{
    public class Log : IEnterAreaEvent
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

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            Debug.Log(message);
            return UniTask.CompletedTask;
        }
    }
}
