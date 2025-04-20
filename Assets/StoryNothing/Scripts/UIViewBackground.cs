using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using UnityEngine;

namespace StoryNothing
{
    public class UIViewBackground
    {
        public readonly HKUIDocument backgroundDocumentPrefab;

        private HKUIDocument backgroundDocument;

        public UIViewBackground(HKUIDocument backgroundDocumentPrefab)
        {
            this.backgroundDocumentPrefab = backgroundDocumentPrefab;
        }

        public void Setup(CancellationToken cancellationToken)
        {
            backgroundDocument = Object.Instantiate(backgroundDocumentPrefab);
            backgroundDocument.gameObject.SetActive(true);
            cancellationToken.RegisterWithoutCaptureExecutionContext(() =>
            {
                backgroundDocument.DestroySafe();
            });
        }

        public void Open()
        {
            if (backgroundDocument == null)
            {
                Debug.LogError("Background document is not set up.");
                return;
            }

            backgroundDocument.gameObject.SetActive(true);
        }

        public void Close()
        {
            if (backgroundDocument == null)
            {
                Debug.LogError("Background document is not set up.");
                return;
            }

            backgroundDocument.gameObject.SetActive(false);
        }
    }
}
