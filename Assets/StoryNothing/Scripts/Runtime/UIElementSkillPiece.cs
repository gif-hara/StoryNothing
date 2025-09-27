using System;
using System.Collections.Generic;
using System.Linq;
using HK;
using UnityEngine;

namespace StoryNothing
{
    public readonly struct UIElementSkillPiece : IDisposable
    {
        private readonly HKUIDocument document;

        private readonly HKUIDocument cellPrefab;

        private readonly RectTransform rectTransform;

        private readonly List<UIElementCell> cells;

        public UIElementSkillPiece(HKUIDocument document)
        {
            this.document = document;
            cellPrefab = document.Q<HKUIDocument>("UI.Element.Cell");
            rectTransform = document.GetComponent<RectTransform>();
            cells = new List<UIElementCell>();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(document.gameObject);
        }

        public void Setup(List<Vector2Int> cellPoints)
        {
            foreach (var cell in cells)
            {
                cell.Dispose();
            }
            cells.Clear();
            var xMax = cellPoints.Count > 0 ? cellPoints.Max(x => x.x) + 1 : 0;
            var yMax = cellPoints.Count > 0 ? cellPoints.Max(x => x.y) + 1 : 0;
            foreach (var cellPoint in cellPoints)
            {
                var cellDocument = UnityEngine.Object.Instantiate(cellPrefab, rectTransform);
                var cell = new UIElementCell(cellDocument);
                cell.SetPosition(cellPoint, xMax, yMax);
                cells.Add(cell);
            }
        }

        public void SetBackgroundColor(Define.SkillPieceColor colorType)
        {
            foreach (var cell in cells)
            {
                cell.SetBackgroundColor(colorType);
            }
        }

        public void SetPosition(Vector2Int index, int xMax, int yMax)
        {
            rectTransform.localPosition = new Vector3(index.x * 100 - (xMax * 100) / 2 + 50, index.y * 100 - (yMax * 100) / 2 + 50, 0);
        }
        public void SetPositionInCenter()
        {
            rectTransform.localPosition = Vector3.zero;
        }
    }
}
