using System;
using HK;

namespace StoryNothing
{
    public readonly struct UIElementBingoLine : IDisposable
    {
        private readonly HKUIDocument document;

        public UIElementBingoLine(HKUIDocument document)
        {
            this.document = document;
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(document.gameObject);
        }
    }
}
