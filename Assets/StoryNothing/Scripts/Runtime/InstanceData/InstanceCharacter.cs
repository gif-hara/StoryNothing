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

        public readonly CharacterParameter CurrentAttack;

        public readonly CharacterParameter CurrentDefense;

        public readonly CharacterParameter CurrentSpeed;

        public InstanceCharacter(int characterSpecId)
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
            CurrentHitPointMax = new CharacterParameter(CharacterSpec.HitPoint);
            CurrentHitPoint = CurrentHitPointMax.Current;
            CurrentAttack = new CharacterParameter(CharacterSpec.Attack);
            CurrentDefense = new CharacterParameter(CharacterSpec.Defense);
            CurrentSpeed = new CharacterParameter(CharacterSpec.Speed);
        }

        public InstanceCharacter(
            int characterSpecId,
            CharacterParameter currentHitPointMax,
            CharacterParameter currentPhysicalAttack,
            CharacterParameter currentPhysicalDefense,
            CharacterParameter currentSpeed
            )
        {
            CharacterSpecId = characterSpecId;
            CharacterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(CharacterSpecId);
            CurrentHitPointMax = currentHitPointMax;
            CurrentHitPoint = CurrentHitPointMax.Current;
            CurrentAttack = currentPhysicalAttack;
            CurrentDefense = currentPhysicalDefense;
            CurrentSpeed = currentSpeed;
        }
    }
}
