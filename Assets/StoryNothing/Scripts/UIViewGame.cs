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

        private HKUIDocument buttonPrefab;

        public UIViewGame(HKUIDocument backgroundDocumentPrefab)
        {
            this.documentPrefab = backgroundDocumentPrefab;
        }

        public void Setup(CancellationToken cancellationToken)
        {
            document = Object.Instantiate(documentPrefab);
            document.gameObject.SetActive(true);
            areaButtonsDocument = document.Q<HKUIDocument>("Area.Buttons");
            buttonPrefab = areaButtonsDocument.Q<HKUIDocument>("Prefab.Button");
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

        public async UniTask<int> CreateButtons(IEnumerable<string> buttonTexts, CancellationToken cancellationToken)
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
                    var button = Object.Instantiate(buttonPrefab, areaButtonsDocument.transform);
                    button.Q<TMP_Text>("Text").text = x;
                    return button.Q<Button>("Button").OnClickAsync();
                })
            );
        }

        public void DestroyButtonAll()
        {
            for (var i = 0; i < areaButtonsDocument.transform.childCount; i++)
            {
                var child = areaButtonsDocument.transform.GetChild(i);
                if (child != null)
                {
                    Object.Destroy(child.gameObject);
                }
            }
        }
    }
}
