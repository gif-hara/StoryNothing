using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class CharacterSpec
    {
        public int Id;

        public int HitPoint;

        public int Attack;

        public int Defense;

        public int Speed;

        [Serializable]
        public class DictionaryList : DictionaryList<int, CharacterSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
