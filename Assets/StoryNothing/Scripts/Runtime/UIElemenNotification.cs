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
            var text = labelInstance.Q<TMP_Text>("Message");
            text.text = message;
            var content = labelInstance.Q<RectTransform>("Contents");
            var canvasGroup = labelInstance.Q<CanvasGroup>("Contents");

            await LMotion.Create(0.0f, 1.0f, 0.2f)
                .WithEase(Ease.OutQuad)
                .Bind(content, static (x, content) =>
                {
                    var anchorMin = content.anchorMin;
                    anchorMin.x = Mathf.Lerp(1.0f, 0.0f, x);
                    content.anchorMin = anchorMin;
                    var anchorMax = content.anchorMax;
                    anchorMax.x = Mathf.Lerp(2.0f, 1.0f, x);
                    content.anchorMax = anchorMax;
                })
                .ToUniTask(cancellationToken: cancellationToken);
            await UniTask.Delay(3000, cancellationToken: cancellationToken);
            await LMotion.Create(1.0f, 0.0f, 0.2f)
                .WithEase(Ease.InQuad)
                .Bind(canvasGroup, static (x, canvasGroup) =>
                {
                    canvasGroup.alpha = x;
                })
                .ToUniTask(cancellationToken: cancellationToken);

            Object.Destroy(labelInstance.gameObject);
        }
    }
}
