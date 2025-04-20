using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StoryNothing.AreaControllers.EnterAreaEvents
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

        public async UniTask EnterAsync(IGameController gameController, CancellationToken cancellationToken)
        {
            var result = await gameController.CreateButtonsAsync(buttonDatabase.Select(data => data.ButtonText), cancellationToken);
            if (result < 0 || result >= buttonDatabase.Count)
            {
                Debug.LogError($"Invalid button index: {result}");
                return;
            }
            for (var i = 0; i < buttonDatabase[result].OnClickEvents.Count; i++)
            {
                var e = buttonDatabase[result].OnClickEvents[i];
                if (e == null || e.Value == null)
                {
                    Debug.LogError($"Button {i} is null.");
                    continue;
                }

                await e.Value.EnterAsync(gameController, cancellationToken);
            }
        }
    }
}
