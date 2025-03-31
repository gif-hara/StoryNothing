using System;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    [Serializable]
    public class BattleActorSpec
    {
        public string name;

        public int hitPoint;

        public int physicalAttack;

        public int magicAttack;

        public int physicalDefense;

        public int magicDefense;

        public int speed;

        public BattleActorSpec Clone()
        {
            return new BattleActorSpec
            {
                name = name,
                hitPoint = hitPoint,
                physicalAttack = physicalAttack,
                magicAttack = magicAttack,
                physicalDefense = physicalDefense,
                magicDefense = magicDefense,
                speed = speed
            };
        }
    }
}
