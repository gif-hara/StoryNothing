using HK;
using StoryNothing.MasterDataSystems;

namespace StoryNothing.InstanceData
{
    public sealed class InstanceCharacter
    {
        public int CharacterSpecId { get; private set; }

        public CharacterSpec CharacterSpec { get; private set; }

        public readonly CharacterParameter CurrentHitPoint;

        public readonly CharacterParameter CurrentMagicPoint;

        public readonly CharacterParameter CurrentPhysicalAttack;

        public readonly CharacterParameter CurrentPhysicalDefense;

        public readonly CharacterParameter CurrentMagicalAttack;

        public readonly CharacterParameter CurrentMagicalDefense;

        public readonly CharacterParameter CurrentSpeed;

        public InstanceCharacter(int characterSpecId)
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
            CurrentHitPoint = new CharacterParameter(CharacterSpec.HitPoint);
            CurrentMagicPoint = new CharacterParameter(CharacterSpec.MagicPoint);
            CurrentPhysicalAttack = new CharacterParameter(CharacterSpec.PhysicalAttack);
            CurrentPhysicalDefense = new CharacterParameter(CharacterSpec.PhysicalDefense);
            CurrentMagicalAttack = new CharacterParameter(CharacterSpec.MagicalAttack);
            CurrentMagicalDefense = new CharacterParameter(CharacterSpec.MagicalDefense);
            CurrentSpeed = new CharacterParameter(CharacterSpec.Speed);
        }
    }
}
