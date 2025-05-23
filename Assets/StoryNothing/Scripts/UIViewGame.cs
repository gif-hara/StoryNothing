using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using R3;
using R3.Triggers;
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

        private HKUIDocument buttonListPrefab;

        private HKUIDocument buttonPrefab;

        private HKUIDocument areaMessageDocument;

        private Transform messageParent;

        private HKUIDocument messagePrefab;

        private Button backButton;

        private readonly List<HKUIDocument> buttonParents = new();

        private bool isButtonsNoStack = false;

        public UIViewGame(HKUIDocument documentPrefab)
        {
            this.documentPrefab = documentPrefab;
        }

        public void Setup(IGameController gameController, CancellationToken cancellationToken)
        {
            document = Object.Instantiate(documentPrefab);
            document.gameObject.SetActive(true);
            areaButtonsDocument = document.Q<HKUIDocument>("Area.Buttons");
            buttonListPrefab = areaButtonsDocument.Q<HKUIDocument>("Prefab.List");
            buttonPrefab = areaButtonsDocument.Q<HKUIDocument>("Prefab.Button");
            areaMessageDocument = document.Q<HKUIDocument>("Area.Messages");
            messageParent = areaMessageDocument.Q<Transform>("Parent.Message");
            messagePrefab = areaMessageDocument.Q<HKUIDocument>("Prefab.Message");
            var areaSystemsDocument = document.Q<HKUIDocument>("Area.Systems");
            var systemDefaultDocument = areaSystemsDocument.Q<HKUIDocument>("Default");
            backButton = systemDefaultDocument.Q<HKUIDocument>("Back").Q<Button>("Button");
            backButton
                .OnClickAsObservable()
                .Subscribe(gameController, static (_, gameController) =>
                {
                    gameController.PopButtons();
                })
                .RegisterTo(cancellationToken);
            systemDefaultDocument
                .Q<HKUIDocument>("Item")
                .Q<Button>("Button")
                .OnClickAsObservable()
                .Subscribe((gameController, cancellationToken), static (_, t) =>
                {
                    var (gameController, cancellationToken) = t;
                    gameController.PushButtonsNoStack(gameController.CreateUserDataItemsButtonDatabase(), cancellationToken);
                })
                .RegisterTo(cancellationToken);
            cancellationToken.RegisterWithoutCaptureExecutionContext(() =>
            {
                document.DestroySafe();
            });
        }

        public void Open()
        {
            document.gameObject.SetActive(true);
        }

        public void Close()
        {
            document.gameObject.SetActive(false);
        }

        public void PushButtons(IEnumerable<CreateButtonData> buttonDatabase, IGameController gameController, CancellationToken cancellationToken)
        {
            DestroyNoStackButtonParent();
            isButtonsNoStack = false;
            CreateButtons(buttonDatabase, gameController, cancellationToken);
            backButton.gameObject.SetActive(buttonParents.Count > 1);
        }

        public void PushButtonsNoStack(IEnumerable<CreateButtonData> buttonDatabase, IGameController gameController, CancellationToken cancellationToken)
        {
            DestroyNoStackButtonParent();
            isButtonsNoStack = true;
            CreateButtons(buttonDatabase, gameController, cancellationToken);
            backButton.gameObject.SetActive(buttonParents.Count > 1);
        }

        private void DestroyNoStackButtonParent()
        {
            if (isButtonsNoStack)
            {
                var buttonParent = buttonParents[^1];
                buttonParent.DestroySafe();
                buttonParents.RemoveAt(buttonParents.Count - 1);
            }
        }

        private void CreateButtons(IEnumerable<CreateButtonData> buttonDatabase, IGameController gameController, CancellationToken cancellationToken)
        {
            var parentDocument = Object.Instantiate(buttonListPrefab, areaButtonsDocument.transform);
            var content = parentDocument.Q<Transform>("Content");
            buttonParents.Add(parentDocument);
            buttonDatabase = buttonDatabase.Where(x =>
            {
                if (x.CanCreate == null || x.CanCreate.Value == null)
                {
                    return true;
                }
                return x.CanCreate.Value.Evaluate(gameController);
            });
            foreach (var data in buttonDatabase)
            {
                var buttonDocument = Object.Instantiate(buttonPrefab, content);
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
            if (buttonParents.Count <= 1)
            {
                return;
            }
            isButtonsNoStack = false;
            var parent = buttonParents[^1];
            buttonParents.RemoveAt(buttonParents.Count - 1);
            parent.DestroySafe();
            backButton.gameObject.SetActive(buttonParents.Count > 1);
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
