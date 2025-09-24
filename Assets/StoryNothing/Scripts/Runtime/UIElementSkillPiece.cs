using System;
using HK;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing
{
    public readonly struct UIElementSkillPiece : IDisposable
    {
        private readonly HKUIDocument document;

        private readonly Image backgroundImage;

        private readonly RectTransform rectTransform;

        public UIElementSkillPiece(HKUIDocument document)
        {
            this.document = document;
            backgroundImage = document.Q<Image>("Background");
            rectTransform = document.GetComponent<RectTransform>();
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
    }
}
