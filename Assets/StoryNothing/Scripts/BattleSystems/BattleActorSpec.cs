using System;
using UnityEngine;

namespace StoryNothing.BattleSystems
{
    [Serializable]
    public class BattleActorSpec
    {
        [SerializeField]
        private string name;
        public string Name => name;

        [SerializeField]
        private int hitPoint;
        public int HitPoint => hitPoint;

        [SerializeField]
        private int physicalAttack;
        public int PhysicalAttack => physicalAttack;

        [SerializeField]
        private int magicAttack;
        public int MagicAttack => magicAttack;

        [SerializeField]
        private int physicalDefense;
        public int PhysicalDefense => physicalDefense;

        [SerializeField]
        private int magicDefense;
        public int MagicDefense => magicDefense;

        [SerializeField]
        private int speed;
        public int Speed => speed;
    }
}
