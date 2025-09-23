using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCell
    {
        public int Id;

        public int X;

        public int Y;

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillPieceCell>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
