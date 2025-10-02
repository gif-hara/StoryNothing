using UnityEngine;

namespace StoryNothing
{
    public static class Define
    {
        public enum ButtonBehavior
        {
            OnClick,
            OnPointerEnter,
            OnPointerExit,
        }

        public enum SkillPieceColor
        {
            Gray,
            Red,
        }

        public enum Direction
        {
            Right = 0,
            Top = 1,
            Left = 2,
            Bottom = 3,
        }
    }
}
