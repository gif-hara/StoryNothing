using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using LitMotion;
using TMPro;
using UnityEngine;

namespace StoryNothing
{
    public sealed class UIElementNotification
    {
        private readonly HKUIDocument document;

        private readonly RectTransform labelParent;

        private readonly HKUIDocument labelPrefab;

        public UIElementNotification(HKUIDocument document)
        {
            this.document = document;
            labelParent = document.Q<RectTransform>("Area.Label");
            labelPrefab = document.Q<HKUIDocument>("UI.Element.Notification");
        }

        public void Add(string message)
        {
            AddInternalAsync(message, document.destroyCancellationToken).Forget();
        }

        private async UniTask AddInternalAsync(string message, CancellationToken cancellationToken)
        {
            var labelInstance = Object.Instantiate(labelPrefab, labelParent);
            var textComponent = labelInstance.Q<TMP_Text>("Message");
            textComponent.text = message;

            await UniTask.Delay(3000, cancellationToken: cancellationToken);

            Object.Destroy(labelInstance.gameObject);
        }
    }
}
