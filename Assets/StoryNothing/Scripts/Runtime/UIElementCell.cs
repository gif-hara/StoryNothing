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
                Define.SkillPieceColor.Gray => Color.gray,
                Define.SkillPieceColor.Red => Color.red,
                _ => throw new System.ArgumentOutOfRangeException(nameof(colorType), colorType, null),
            };
        }

        public void SetPosition(Vector2Int index, int xMax, int yMax)
        {
            rectTransform.localPosition = new Vector3(index.x * 100 - (xMax * 100) / 2 + 50, index.y * 100 - (yMax * 100) / 2 + 50, 0);
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
