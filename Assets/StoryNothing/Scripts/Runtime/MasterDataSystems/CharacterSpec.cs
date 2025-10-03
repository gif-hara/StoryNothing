namespace StoryNothing.MasterDataSystems
{
    public sealed class CharacterSpec
    {
        public int HitPoint { get; private set; }

        public int MagicPoint { get; private set; }

        public int PhysicalAttack { get; private set; }

        public int PhysicalDefense { get; private set; }

        public int MagicalAttack { get; private set; }

        public int MagicalDefense { get; private set; }

        public int Speed { get; private set; }

        public CharacterSpec(int hitPoint, int magicPoint, int physicalAttack, int physicalDefense, int magicalAttack, int magicalDefense, int speed)
        {
            HitPoint = hitPoint;
            MagicPoint = magicPoint;
            PhysicalAttack = physicalAttack;
            PhysicalDefense = physicalDefense;
            MagicalAttack = magicalAttack;
            MagicalDefense = magicalDefense;
            Speed = speed;
        }
    }
}
