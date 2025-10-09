using UnityEngine;

namespace StoryNothing
{
    public static class Define
    {
        public const float CellSize = 90.0f;

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
            Orange,
            WhiteGray,
            Purple,
            Water,
            Green,
        }

        public enum Direction
        {
            Right = 0,
            Top = 1,
            Left = 2,
            Bottom = 3,
        }

        public enum SkillEffectType
        {
            PhysicalAttackUp,
            PhysicalDefenseUp,
            MagicalAttackUp,
            MagicalDefenseUp,
            HitPointUp,
            SpeedUp,
        }
    }
}
