using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class CharacterSpec
    {
        public int Id;

        public int HitPoint;

        public int MagicPoint;

        public int PhysicalAttack;

        public int PhysicalDefense;

        public int MagicalAttack;

        public int MagicalDefense;

        public int Speed;

        public CharacterSpec(int id, int hitPoint, int magicPoint, int physicalAttack, int physicalDefense, int magicalAttack, int magicalDefense, int speed)
        {
            Id = id;
            HitPoint = hitPoint;
            MagicPoint = magicPoint;
            PhysicalAttack = physicalAttack;
            PhysicalDefense = physicalDefense;
            MagicalAttack = magicalAttack;
            MagicalDefense = magicalDefense;
            Speed = speed;
        }

        [Serializable]
        public class DictionaryList : DictionaryList<int, CharacterSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
