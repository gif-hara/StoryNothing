using System;
using System.Collections.Generic;
using System.Linq;
using HK;
using StoryNothing.InstanceData;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StoryNothing
{
    public class UIElementSkillPiece : IDisposable
    {
        private readonly HKUIDocument document;

        private readonly HKUIDocument cellPrefab;

        private readonly RectTransform rectTransform;

        private readonly List<UIElementCell> cells;

        public InstanceSkillPiece InstanceSkillPiece { get; private set; }

        public UIElementSkillPiece(HKUIDocument document)
        {
            this.document = document;
            cellPrefab = document.Q<HKUIDocument>("UI.Element.Cell");
            rectTransform = document.GetComponent<RectTransform>();
            cells = new List<UIElementCell>();
            InstanceSkillPiece = null;
        }

        public void Dispose()
        {
            if (document == null)
            {
                return;
            }
            UnityEngine.Object.Destroy(document.gameObject);
        }

        public void Setup(InstanceSkillPiece instanceSkillPiece, int rotationIndex)
        {
            InstanceSkillPiece = instanceSkillPiece;
            foreach (var cell in cells)
            {
                cell.Dispose();
            }
            cells.Clear();
            var cellPoints = instanceSkillPiece.SkillPieceCellSpec.GetCellPoints(rotationIndex);
            var xMax = cellPoints.Count > 0 ? cellPoints.Max(x => x.x) + 1 : 0;
            var yMax = cellPoints.Count > 0 ? cellPoints.Max(x => x.y) + 1 : 0;
            foreach (var cellPoint in cellPoints)
            {
                var cellDocument = UnityEngine.Object.Instantiate(cellPrefab, rectTransform);
                var cell = new UIElementCell(cellDocument);
                cell.SetPosition(cellPoint, xMax, yMax);
                cells.Add(cell);
                for (var i = 0; i < 4; i++)
                {
                    var offset = (Define.Direction)i switch
                    {
                        Define.Direction.Right => Vector2Int.right,
                        Define.Direction.Top => Vector2Int.up,
                        Define.Direction.Left => Vector2Int.left,
                        Define.Direction.Bottom => Vector2Int.down,
                        _ => throw new ArgumentOutOfRangeException(),
                    };
                    if (cellPoints.Contains(cellPoint + offset))
                    {
                        cell.SetActiveConnector((Define.Direction)i, true);
                    }
                }
            }
            foreach (var cell in cells)
            {
                cell.SetBackgroundColor(instanceSkillPiece.ColorType);
            }
        }

        public Vector2Int GetPositionIndexFromMousePosition(Vector2Int skillboardSize, Vector2Int skillPieceSize)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Mouse.current.position.ReadValue(),
                null,
                out Vector2 localPoint);
            var result = new Vector2Int(
                Mathf.Clamp(Mathf.FloorToInt((localPoint.x + (skillboardSize.x * 100) / 2) / 100), 0, skillboardSize.x - 1),
                Mathf.Clamp(Mathf.FloorToInt((localPoint.y + (skillboardSize.y * 100) / 2) / 100), 0, skillboardSize.y - 1)
                );
            var skillPieceCenterIndex = new Vector2Int(skillPieceSize.x / 2, skillPieceSize.y / 2);
            if (result.x - skillPieceCenterIndex.x < 0)
            {
                result.x = skillPieceCenterIndex.x;
            }
            if (result.x + (skillPieceSize.x - skillPieceCenterIndex.x - 1) >= skillboardSize.x)
            {
                result.x = skillboardSize.x - (skillPieceSize.x - skillPieceCenterIndex.x - 1) - 1;
            }
            if (result.y - skillPieceCenterIndex.y < 0)
            {
                result.y = skillPieceCenterIndex.y;
            }
            if (result.y + (skillPieceSize.y - skillPieceCenterIndex.y - 1) >= skillboardSize.y)
            {
                result.y = skillboardSize.y - (skillPieceSize.y - skillPieceCenterIndex.y - 1) - 1;
            }

            return result;
        }

        public void SetPositionFromMouse(Vector2 offset, Vector2Int skillboardSize, Vector2Int skillPieceSize)
        {
            var positionIndex = GetPositionIndexFromMousePosition(skillboardSize, skillPieceSize);
            SetPosition(positionIndex, skillboardSize, skillPieceSize);
            rectTransform.localPosition += (Vector3)offset;
        }

        public void SetPosition(Vector2Int positionIndex, Vector2Int skillboardSize, Vector2Int skillPieceSize)
        {
            var result = new Vector3(positionIndex.x * 100 - (skillboardSize.x * 100) / 2 + 50, positionIndex.y * 100 - (skillboardSize.y * 100) / 2 + 50, 0);
            if (skillPieceSize.x % 2 == 0)
            {
                result.x -= 50;
            }
            if (skillPieceSize.y % 2 == 0)
            {
                result.y -= 50;
            }
            rectTransform.localPosition = result;
        }

        public void SetPositionInCenter()
        {
            rectTransform.localPosition = Vector3.zero;
        }

        public Vector3 WorldToScreenPoint()
        {
            return RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
        }

        public void Clear()
        {
            foreach (var cell in cells)
            {
                cell.Dispose();
            }
            cells.Clear();
        }

        public void SetParent(Transform parent)
        {
            rectTransform.SetParent(parent);
        }
    }
}
