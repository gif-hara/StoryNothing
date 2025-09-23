using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceSpec
    {
        public int Id;

        public string Name;

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillPieceSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
