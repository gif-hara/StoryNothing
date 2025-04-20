using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.AreaControllers
{
    public class EnterAreaLog : IEnterArea
    {
        [SerializeField]
        private string message;

        public EnterAreaLog()
        {
        }

        public EnterAreaLog(string logMessage)
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
