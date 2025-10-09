using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using LitMotion;
using LitMotion.Animation;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing
{
    public readonly struct UIElementCell : IDisposable
    {
        private readonly HKUIDocument document;

        private readonly Image backgroundImage;

        private readonly RectTransform rectTransform;

        private readonly List<GameObject> connectors;

        private readonly Transform line;

        public UIElementCell(HKUIDocument document)
        {
            this.document = document;
            backgroundImage = document.Q<Image>("Background");
            rectTransform = document.GetComponent<RectTransform>();
            var connectorDocument = document.Q<HKUIDocument>("Connectors");
            connectors = new List<GameObject>
            {
                connectorDocument.Q("Right"),
                connectorDocument.Q("Top"),
                connectorDocument.Q("Left"),
                connectorDocument.Q("Bottom"),
            };
            foreach (var connector in connectors)
            {
                connector.SetActive(false);
            }
            line = document.Q<Transform>("Line");
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(document.gameObject);
        }

        public void SetBackgroundColor(Define.SkillPieceColor colorType)
        {
            backgroundImage.color = colorType switch
            {
                Define.SkillPieceColor.Gray => new Color(0.2f, 0.2f, 0.2f),
                Define.SkillPieceColor.Red => new Color(1.0f, 0.2f, 0.2f),
                Define.SkillPieceColor.Orange => new Color(1.0f, 0.6f, 0.2f),
                Define.SkillPieceColor.WhiteGray => new Color(0.9f, 0.9f, 0.9f),
                Define.SkillPieceColor.Purple => new Color(0.6f, 0.2f, 0.9f),
                Define.SkillPieceColor.Water => new Color(0.3f, 0.9f, 1.0f),
                Define.SkillPieceColor.Green => new Color(0.2f, 0.9f, 0.4f),
                _ => throw new System.ArgumentOutOfRangeException(nameof(colorType), colorType, null),
            };
        }

        public void SetPosition(Vector2Int index, int xMax, int yMax)
        {
            var size = Define.CellSize;
            var halfSize = size / 2;
            rectTransform.localPosition = new Vector3(index.x * size - (xMax * size) / 2 + halfSize, index.y * size - (yMax * size) / 2 + halfSize, 0);
        }

        public void SetActiveConnector(Define.Direction direction, bool active)
        {
            connectors[(int)direction].SetActive(active);
        }

        public UniTask PlayLineAnimationAsync(CancellationToken cancellationToken)
        {
            return LMotion.Create(new Vector3(-50.0f, 50.0f, 0.0f), new Vector3(50.0f, -50.0f, 0.0f), 0.2f)
                .BindToLocalPosition(line)
                .ToUniTask(cancellationToken: cancellationToken);
        }
    }
}
