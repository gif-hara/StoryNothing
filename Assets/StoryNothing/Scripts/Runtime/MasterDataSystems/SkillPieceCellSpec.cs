using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCellSpec
    {
        public int Id;

        public int X;

        public int Y;

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillPieceCellSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
