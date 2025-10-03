using HK;
using StoryNothing.MasterDataSystems;
using UnityEngine;

namespace StoryNothing.InstanceData
{
    public sealed class InstanceCharacter
    {
        public int CharacterSpecId { get; private set; }

        public CharacterSpec CharacterSpec { get; private set; }

        public int HitPointMax => CharacterSpec.HitPoint + AdditionalHitPoint + (int)(CharacterSpec.HitPoint * AdditionalHitPointRate);

        public int MagicPointMax => CharacterSpec.MagicPoint + AdditionalMagicPoint + (int)(CharacterSpec.MagicPoint * AdditionalMagicPointRate);

        public int CurrentPhysicalAttack => CharacterSpec.PhysicalAttack + AdditionalPhysicalAttack + (int)(CharacterSpec.PhysicalAttack * AdditionalPhysicalAttackRate);

        public int CurrentPhysicalDefense => CharacterSpec.PhysicalDefense + AdditionalPhysicalDefense + (int)(CharacterSpec.PhysicalDefense * AdditionalPhysicalDefenseRate);

        public int CurrentMagicalAttack => CharacterSpec.MagicalAttack + AdditionalMagicalAttack + (int)(CharacterSpec.MagicalAttack * AdditionalMagicalAttackRate);

        public int CurrentMagicalDefense => CharacterSpec.MagicalDefense + AdditionalMagicalDefense + (int)(CharacterSpec.MagicalDefense * AdditionalMagicalDefenseRate);

        public int CurrentSpeed => CharacterSpec.Speed + AdditionalSpeed + (int)(CharacterSpec.Speed * AdditionalSpeedRate);

        public int AdditionalHitPoint { get; set; }

        public int AdditionalMagicPoint { get; set; }

        public int AdditionalPhysicalAttack { get; set; }

        public int AdditionalPhysicalDefense { get; set; }

        public int AdditionalMagicalAttack { get; set; }

        public int AdditionalMagicalDefense { get; set; }

        public int AdditionalSpeed { get; set; }

        public float AdditionalHitPointRate { get; set; }

        public float AdditionalMagicPointRate { get; set; }

        public float AdditionalPhysicalAttackRate { get; set; }

        public float AdditionalPhysicalDefenseRate { get; set; }

        public float AdditionalMagicalAttackRate { get; set; }

        public float AdditionalMagicalDefenseRate { get; set; }

        public float AdditionalSpeedRate { get; set; }

        public InstanceCharacter(int characterSpecId)
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
        }
    }
}
