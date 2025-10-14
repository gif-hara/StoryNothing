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
            AttackUp,
            DefenseUp,
            HitPointUp,
            SpeedUp,
            AttackCommand,
            DamageUpFormless,
            DamageUpHuman,
            DamageUpDragon,
            DamageUpBird,
            DamageUpBeast,
            RegisterUpPoison,
            RegisterUpParalysis,
            RegisterUpAttackDown,
            RegisterUpDefenseDown,
            BattleStartCommandGaugeUp,
            GiveDamageRecoveryHitPoint,
            TakeDamageCommandGaugeUp,
            GivePoisonAttackUp,
            TakeDeathAvoidAndReciveryHitPoint,
            GiveDamageChainedAttack,
            TakeAbnormalStatusSpeedUp,
            GiveDamageAddPoison,
            GiveDamageAddParalysis,
            GiveDamageAddAttackDown,
            GiveDamageAddDefenseDown,
            Avoid,
            CriticalDamage,
            GiveDamageCommandGaugeUp,
            BingoBonusAttackUp,
            BingoBonusDefenseUp,
            BingoBonusHitPointUp,
            BingoBonusSpeedUp,
            BingoBonusChainedAttack,
            Regain,
            TakeDamageDefenseUp,
            RedPieceSpeedUp,
            GreenPieceHitPointUp,
            OrangePieceMagicalDefenseUp,
            WhiteGrayPieceMagicalAttackUp,
            GiveDamageCommandGaugeDown,
            GiveDamageGuardPointUp,
            BattleStartGuardPointUp,
        }
    }
}
