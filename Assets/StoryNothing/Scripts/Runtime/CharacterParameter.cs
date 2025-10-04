namespace StoryNothing
{
    public struct CharacterParameter
    {
        public int Base;

        public int Additional;

        public float AdditionalRate;

        public int Current => Base + Additional + (int)((Base + Additional) * AdditionalRate);

        public CharacterParameter(int baseValue)
        {
            Base = baseValue;
            Additional = 0;
            AdditionalRate = 0f;
        }

        public CharacterParameter(int baseValue, int additional, float additionalRate)
        {
            Base = baseValue;
            Additional = additional;
            AdditionalRate = additionalRate;
        }
    }
}
