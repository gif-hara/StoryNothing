using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing
{
    public sealed class UIElementDialog
    {
        private readonly HKUIDocument document;

        private readonly RectTransform buttonParent;

        private readonly HKUIDocument buttonPrefab;

        private readonly TMP_Text message;

        public UIElementDialog(HKUIDocument document)
        {
            this.document = document;
            buttonParent = document.Q<RectTransform>("Area.Button");
            buttonPrefab = document.Q<HKUIDocument>("UI.Element.Button");
            message = document.Q<TMP_Text>("Message");
            document.gameObject.SetActive(false);
        }

        public async UniTask<int> ShowAsync(string message, IEnumerable<string> buttonMessages, CancellationToken cancellationToken)
        {
            document.gameObject.SetActive(true);
            this.message.text = message;

            var buttons = new List<HKUIDocument>();
            foreach (var buttonMessage in buttonMessages)
            {
                var button = Object.Instantiate(buttonPrefab, buttonParent);
                button.Q<TMP_Text>("Text").text = buttonMessage;
                buttons.Add(button);
            }

            using var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await UniTask.WhenAny(buttons.Select(b => b.Q<Button>("Button").OnClickAsync(cancellationToken: scope.Token)));
            scope.Cancel();
            foreach (var button in buttons)
            {
                Object.Destroy(button.gameObject);
            }
            document.gameObject.SetActive(false);
            return result;
        }
    }
}
