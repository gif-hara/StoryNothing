using System;
using HK;
using UnityEngine;

namespace StoryNothing
{
    public readonly struct UIElementBingoLine : IDisposable
    {
        private readonly HKUIDocument document;

        private readonly RectTransform rectTransform;

        public UIElementBingoLine(HKUIDocument document)
        {
            this.document = document;
            this.rectTransform = document.GetComponent<RectTransform>();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(document.gameObject);
        }

        public void SetupAsHorizontalLine(int index, int yMax)
        {
            var cellSize = Define.CellSize;
            rectTransform.localPosition = new Vector3(0, -((yMax - 1) * cellSize / 2f - index * cellSize), 0);
        }

        public void SetupAsVerticalLine(int index, int xMax)
        {
            var cellSize = Define.CellSize;
            rectTransform.localPosition = new Vector3(-(xMax - 1) * cellSize / 2f + index * cellSize, 0, 0);
            rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }
}
