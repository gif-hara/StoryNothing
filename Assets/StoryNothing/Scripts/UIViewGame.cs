using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing
{
    public class UIViewGame
    {
        public readonly HKUIDocument documentPrefab;

        private HKUIDocument document;

        private HKUIDocument areaButtonsDocument;

        private Transform buttonParent;

        private HKUIDocument buttonPrefab;

        private HKUIDocument areaMessageDocument;

        private Transform messageParent;

        private HKUIDocument messagePrefab;

        public UIViewGame(HKUIDocument backgroundDocumentPrefab)
        {
            this.documentPrefab = backgroundDocumentPrefab;
        }

        public void Setup(CancellationToken cancellationToken)
        {
            document = UnityEngine.Object.Instantiate(documentPrefab);
            document.gameObject.SetActive(true);
            areaButtonsDocument = document.Q<HKUIDocument>("Area.Buttons");
            buttonParent = areaButtonsDocument.Q<Transform>("Parent.Buttons");
            buttonPrefab = areaButtonsDocument.Q<HKUIDocument>("Prefab.Button");
            areaMessageDocument = document.Q<HKUIDocument>("Area.Messages");
            messageParent = areaMessageDocument.Q<Transform>("Parent.Message");
            messagePrefab = areaMessageDocument.Q<HKUIDocument>("Prefab.Message");
            cancellationToken.RegisterWithoutCaptureExecutionContext(() =>
            {
                document.DestroySafe();
            });
        }

        public void Open()
        {
            if (document == null)
            {
                Debug.LogError("Background document is not set up.");
                return;
            }

            document.gameObject.SetActive(true);
        }

        public void Close()
        {
            if (document == null)
            {
                Debug.LogError("Background document is not set up.");
                return;
            }

            document.gameObject.SetActive(false);
        }

        public async UniTask<int> CreateButtonsAsync(IEnumerable<string> buttonTexts, CancellationToken cancellationToken)
        {
            if (buttonTexts == null)
            {
                Debug.LogError("Button texts cannot be null.");
                return -1;
            }

            DestroyButtonAll();

            return await UniTask.WhenAny(buttonTexts
                .Select(x =>
                {
                    var button = UnityEngine.Object.Instantiate(buttonPrefab, buttonParent);
                    button.Q<TMP_Text>("Text").text = x;
                    return button.Q<Button>("Button").OnClickAsync();
                })
            );
        }

        public void DestroyButtonAll()
        {
            for (var i = 0; i < buttonParent.childCount; i++)
            {
                var child = buttonParent.GetChild(i);
                if (child != null)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
        }

        public UniTask CreateMessageAsync(string message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError("Message cannot be null or empty.");
                return UniTask.CompletedTask;
            }

            var messageDocument = UnityEngine.Object.Instantiate(messagePrefab, messageParent);
            messageDocument.Q<TMP_Text>("Text").text = message;
            return UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: cancellationToken);
        }
    }
}
