using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillBoardSpec
    {
        public int Id;

        public int X;

        public int Y;

        public int HoleCount;

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillBoardSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
