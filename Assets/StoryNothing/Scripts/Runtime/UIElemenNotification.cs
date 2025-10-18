using HK;
using TMPro;
using UnityEngine;

namespace StoryNothing
{
    public sealed class UIElementNotification
    {
        private readonly RectTransform labelParent;

        private readonly HKUIDocument labelPrefab;

        public UIElementNotification(HKUIDocument document)
        {
            labelParent = document.Q<RectTransform>("Area.Label");
            labelPrefab = document.Q<HKUIDocument>("UI.Element.Notification");
        }

        public void Add(string message)
        {
            var labelInstance = Object.Instantiate(labelPrefab, labelParent);
            var textComponent = labelInstance.Q<TMP_Text>("Text");
            textComponent.text = message;
        }
    }
}
