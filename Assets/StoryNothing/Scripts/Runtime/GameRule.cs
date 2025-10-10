using UnityEngine;

namespace StoryNothing
{
    [CreateAssetMenu(fileName = "GameRule", menuName = "StoryNothing/GameRule")]
    public sealed class GameRule : ScriptableObject
    {
        [field: SerializeField]
        public int SkillPieceHitPointUp { get; private set; }

        [field: SerializeField]
        public int SkillPiecePhysicalAttackUp { get; private set; }

        [field: SerializeField]
        public int SkillPiecePhysicalDefenseUp { get; private set; }

        [field: SerializeField]
        public int SkillPieceMagicalAttackUp { get; private set; }

        [field: SerializeField]
        public int SkillPieceMagicalDefenseUp { get; private set; }

        [field: SerializeField]
        public int SkillPieceSpeedUp { get; private set; }
    }
}
