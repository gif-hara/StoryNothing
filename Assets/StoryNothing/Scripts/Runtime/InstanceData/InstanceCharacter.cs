using HK;
using StoryNothing.MasterDataSystems;

namespace StoryNothing.InstanceData
{
    public sealed class InstanceCharacter
    {
        public int CharacterSpecId { get; private set; }

        public CharacterSpec CharacterSpec { get; private set; }

        public readonly CharacterParameter CurrentHitPointMax;

        public int CurrentHitPoint;

        public readonly CharacterParameter CurrentPhysicalAttack;

        public readonly CharacterParameter CurrentPhysicalDefense;

        public readonly CharacterParameter CurrentMagicalAttack;

        public readonly CharacterParameter CurrentMagicalDefense;

        public readonly CharacterParameter CurrentSpeed;

        public InstanceCharacter(int characterSpecId)
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
            CurrentHitPointMax = new CharacterParameter(CharacterSpec.HitPoint);
            CurrentHitPoint = CurrentHitPointMax.Current;
            CurrentPhysicalAttack = new CharacterParameter(CharacterSpec.PhysicalAttack);
            CurrentPhysicalDefense = new CharacterParameter(CharacterSpec.PhysicalDefense);
            CurrentMagicalAttack = new CharacterParameter(CharacterSpec.MagicalAttack);
            CurrentMagicalDefense = new CharacterParameter(CharacterSpec.MagicalDefense);
            CurrentSpeed = new CharacterParameter(CharacterSpec.Speed);
        }

        public InstanceCharacter(
            int characterSpecId,
            CharacterParameter currentHitPointMax,
            CharacterParameter currentPhysicalAttack,
            CharacterParameter currentPhysicalDefense,
            CharacterParameter currentMagicalAttack,
            CharacterParameter currentMagicalDefense,
            CharacterParameter currentSpeed
            )
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
            CurrentHitPointMax = currentHitPointMax;
            CurrentHitPoint = CurrentHitPointMax.Current;
            CurrentPhysicalAttack = currentPhysicalAttack;
            CurrentPhysicalDefense = currentPhysicalDefense;
            CurrentMagicalAttack = currentMagicalAttack;
            CurrentMagicalDefense = currentMagicalDefense;
            CurrentSpeed = currentSpeed;
        }
    }
}
