using HK;
using StoryNothing.MasterDataSystems;
using UnityEngine;

namespace StoryNothing.InstanceData
{
    public sealed class InstanceCharacter
    {
        public int CharacterSpecId { get; private set; }

        public CharacterSpec CharacterSpec { get; private set; }

        public int HitPointMax => CharacterSpec.HitPoint;

        public int MagicPointMax => CharacterSpec.MagicPoint;

        public int CurrentPhysicalAttack => CharacterSpec.PhysicalAttack;

        public int CurrentPhysicalDefense => CharacterSpec.PhysicalDefense;

        public int CurrentMagicalAttack => CharacterSpec.MagicalAttack;

        public int CurrentMagicalDefense => CharacterSpec.MagicalDefense;

        public int CurrentSpeed => CharacterSpec.Speed;

        public InstanceCharacter(int characterSpecId)
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
        }
    }
}
