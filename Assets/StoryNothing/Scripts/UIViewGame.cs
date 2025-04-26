using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using R3;
using R3.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace StoryNothing
{
    public class UIViewGame
    {
        public readonly HKUIDocument documentPrefab;

        private HKUIDocument document;

        private HKUIDocument areaButtonsDocument;

        private HKUIDocument buttonListPrefab;

        private HKUIDocument buttonPrefab;

        private HKUIDocument areaMessageDocument;

        private Transform messageParent;

        private HKUIDocument messagePrefab;

        private readonly List<HKUIDocument> buttonParents = new();

        public UIViewGame(HKUIDocument documentPrefab)
        {
            this.documentPrefab = documentPrefab;
        }

        public void Setup(CancellationToken cancellationToken)
        {
            document = UnityEngine.Object.Instantiate(documentPrefab);
            document.gameObject.SetActive(true);
            areaButtonsDocument = document.Q<HKUIDocument>("Area.Buttons");
            buttonListPrefab = areaButtonsDocument.Q<HKUIDocument>("Prefab.List");
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

        public void PushButtons(IEnumerable<CreateButtonData> buttonDatabase, IGameController gameController, CancellationToken cancellationToken)
        {
            var parentDocument = Object.Instantiate(buttonListPrefab, areaButtonsDocument.transform);
            var content = parentDocument.Q<Transform>("Content");
            buttonParents.Add(parentDocument);
            foreach (var data in buttonDatabase)
            {
                var buttonDocument = UnityEngine.Object.Instantiate(buttonPrefab, content);
                buttonDocument.Q<TMP_Text>("Text").text = data.ButtonText;
                var button = buttonDocument.Q<Button>("Button");
                button.OnClickAsObservable()
                    .Subscribe((data, gameController, cancellationToken), static (_, t) =>
                    {
                        var (data, gameController, cancellationToken) = t;
                        foreach (var e in data.OnClickEvents)
                        {
                            e.Value.InvokeAsync(gameController, cancellationToken).Forget();
                        }
                    })
                    .RegisterTo(cancellationToken);
                button.OnPointerEnterAsObservable()
                    .Subscribe((data, gameController, cancellationToken), static (_, t) =>
                    {
                        var (data, gameController, cancellationToken) = t;
                        foreach (var e in data.OnPointerEnterEvents)
                        {
                            e.Value.InvokeAsync(gameController, cancellationToken).Forget();
                        }
                    })
                    .RegisterTo(cancellationToken);
                button.OnPointerExitAsObservable()
                    .Subscribe((data, gameController, cancellationToken), static (_, t) =>
                    {
                        var (data, gameController, cancellationToken) = t;
                        foreach (var e in data.OnPointerExitEvents)
                        {
                            e.Value.InvokeAsync(gameController, cancellationToken).Forget();
                        }
                    })
                    .RegisterTo(cancellationToken);
            }
        }

        public void PopButtons()
        {
            Assert.IsTrue(buttonParents.Count > 0, "No button parents to pop.");
            var parent = buttonParents[^1];
            buttonParents.RemoveAt(buttonParents.Count - 1);
            parent.DestroySafe();
        }

        public void DestroyButtonAll()
        {
            foreach (var parent in buttonParents)
            {
                parent.DestroySafe();
            }

            buttonParents.Clear();
        }

        public void CreateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError("Message cannot be null or empty.");
                return;
            }

            var messageDocument = UnityEngine.Object.Instantiate(messagePrefab, messageParent);
            messageDocument.Q<TMP_Text>("Text").text = message;
        }
    }
}
