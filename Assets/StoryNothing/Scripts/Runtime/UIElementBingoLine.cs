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

        public void SetupAsHorizontalLine(int index, Vector2Int skillBoardSize)
        {
            var cellSize = Define.CellSize;
            rectTransform.localPosition = new Vector3(0, (skillBoardSize.y - 1) * cellSize / 2f - index * cellSize, 0);
        }

        public void SetupAsVerticalLine(int index, Vector2Int skillBoardSize)
        {
            var cellSize = Define.CellSize;
            rectTransform.localPosition = new Vector3(-(skillBoardSize.x - 1) * cellSize / 2f + index * cellSize, 0, 0);
            rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }
}
